import { Utils } from './Utils';

export class ItemSelector {

    private Element: HTMLElement;
    private ItemSelectorBoxes: HTMLElement[];
    private ItemSelectorRadios: HTMLInputElement[];
    private ItemSelectorRadioNone: HTMLInputElement;

    constructor(itemSelector: HTMLElement = <HTMLElement>document.getElementsByClassName('item-selector')[0]) {
        this.Element = itemSelector;
        this.ItemSelectorBoxes = <HTMLElement[]>Utils.GetArrayFromClass('item-selector-box', this.Element);
        this.ItemSelectorRadios = <HTMLInputElement[]>Utils.GetArrayFromClass('item-selector-radio', this.Element);
        this.ItemSelectorRadioNone = <HTMLInputElement>this.Element.getElementsByClassName('item-selector-radio-none')[0];

        for (const itemSelectorBox of <HTMLElement[]>this.ItemSelectorBoxes) {
            itemSelectorBox.onclick = () => {
                this.SetRadioButton(itemSelectorBox);
                this.SetBorders();
                this.SetPosition();
            }
        };

        this.SetBorders();
        this.SetPosition();
    }

    private SetRadioButton(itemSelectorBox: HTMLElement) {
        const selectedRadio = <HTMLInputElement>itemSelectorBox.getElementsByClassName('item-selector-radio')[0];

        if (selectedRadio.checked) {
            const radioNone = this.ItemSelectorRadioNone;
            radioNone.checked = true;
        } else {
            selectedRadio.checked = true;
        }
    }

    //Setting Borders based on checked radio
    private SetBorders() {
        for (const itemSelectorBox of <HTMLElement[]>this.ItemSelectorBoxes) {
            const radio = <HTMLInputElement>itemSelectorBox.getElementsByClassName('item-selector-radio')[0];

            if (radio.checked) itemSelectorBox!.classList.add('select-checked');
            else itemSelectorBox!.classList.remove('select-checked');
        }
    };

    private SetPosition() {
        for (const itemSelectorRadio of <HTMLInputElement[]>this.ItemSelectorRadios) {
            if (itemSelectorRadio.checked) {
                const box = <HTMLElement>itemSelectorRadio.parentElement!.closest('.item-selector-box');
                this.Element.scrollTo(box.offsetLeft - 30, 0);
            };
        }
    }

}