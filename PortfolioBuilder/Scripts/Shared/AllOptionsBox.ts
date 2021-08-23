import { Utils } from './Utils';

export class AllOptionsBox {

    private Element: HTMLElement;
    private OptionsBoxes: HTMLElement[];
    private OptionsButtons: HTMLElement[];
    private AddOptionButton?: HTMLElement;
    private AddOptionBox?: HTMLElement;
    private DeleteOptionButton?: HTMLElement;
    private DeleteOptionBox?: HTMLElement;
    private AddFunction?: () => void;
    private AddFunctionElse?: () => void;
    private DeleteFunction?: () => void;
    private DeleteFunctionElse?: () => void;

    constructor({ addFunction, addFunctionElse, deleteFunction, deleteFunctionElse }: { addFunction?: () => void, addFunctionElse?: () => void, deleteFunction?: () => void, deleteFunctionElse?: () => void }, allOptionsBox: HTMLElement = <HTMLElement>document.getElementsByClassName('all-options-box')[0]) {
        this.Element = allOptionsBox;
        this.OptionsBoxes = Utils.GetArrayFromClass('option-box', allOptionsBox);
        this.OptionsButtons = Utils.GetArrayFromClass('option-button', allOptionsBox);
        this.AddOptionButton = <HTMLElement>this.Element.getElementsByClassName('add-option-button')[0];
        this.AddOptionBox = <HTMLElement>this.Element.getElementsByClassName('add-option-box')[0];
        this.DeleteOptionButton = <HTMLElement>this.Element.getElementsByClassName('delete-option-button')[0];
        this.DeleteOptionBox = <HTMLElement>this.Element.getElementsByClassName('delete-option-box')[0];

        if (addFunction) this.AddFunction = addFunction;
        if (addFunctionElse) this.AddFunctionElse = addFunctionElse;
        if (deleteFunction) this.DeleteFunction = deleteFunction;
        if (deleteFunctionElse) this.DeleteFunctionElse = deleteFunctionElse;

        //Hide on startup
        Utils.Hide(this.OptionsBoxes);

        if (this.AddOptionButton) this.AddOptionButton.onclick = (event) => this.OptionButtonClick(this.AddOptionButton!, this.AddOptionBox!, event);
        if (this.DeleteOptionButton) this.DeleteOptionButton.onclick = (event) => this.OptionButtonClick(this.DeleteOptionButton!, this.DeleteOptionBox!, event);
    }

    private OptionButtonClick(buttonClicked: HTMLElement, boxToToggle: HTMLElement, event: Event) {
        event.preventDefault();

        Utils.Hide(this.OptionsBoxes.filter(optionBox => optionBox !== boxToToggle));
        this.Element.classList.remove('add-active');
        this.Element.classList.remove('delete-active');

        //Set all other buttons to unclicked
        for (let button of this.OptionsButtons.filter(optionButton => optionButton !== buttonClicked)) {
            button.setAttribute('aria-pressed', 'false');
            button.classList.remove('active');
        }

        Utils.ShowHide([boxToToggle!]);

        const btnClickedClassList = buttonClicked.classList;

        if (btnClickedClassList.contains('add-option-button')) this.AddOptionButtonClick();
        else if (btnClickedClassList.contains('delete-option-button')) this.DeleteOptionButtonClick();
    }

    private AddOptionButtonClick() {
        //Inverse as active not available on click
        if (!this.AddOptionButton!.classList.contains('active')) {
            this.Element.classList.add('add-active');
            if (this.AddFunction) this.AddFunction();
            if (this.DeleteFunctionElse) this.DeleteFunctionElse();
        }
        else {
            if (this.AddFunctionElse) this.AddFunctionElse();
        };
    }

    private DeleteOptionButtonClick() {
        //Inverse as active not available on click
        if (!this.DeleteOptionButton!.classList.contains('active')) {
            this.Element.classList.add('delete-active');
            if (this.DeleteFunction) this.DeleteFunction();
            if (this.AddFunctionElse) this.AddFunctionElse();
        }
        else {
            if (this.DeleteFunctionElse) this.DeleteFunctionElse();
        };
    }

}



