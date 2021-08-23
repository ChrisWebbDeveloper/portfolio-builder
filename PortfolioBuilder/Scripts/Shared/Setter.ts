import { Utils } from "./Utils";

export class Setter {

    private Element: HTMLElement;
    private ItemGetter: HTMLElement;

    constructor(itemGetter: HTMLElement, itemSetter: HTMLElement) {
        this.Element = itemSetter;
        this.ItemGetter = itemGetter;
    }

    public Selected(forDeletion: boolean = false) {
        const getSelected = <HTMLInputElement[]>Utils.GetArrayFromClass('item-setter-get-id', this.ItemGetter);
        const allSetSelected = <HTMLInputElement[]>Utils.GetArrayFromClass('set-selected', this.Element);

        for (const setSelected of allSetSelected) {
            if (forDeletion) {
                const getSelectedInput = <HTMLInputElement>getSelected.find(element => element.value == setSelected.value);
                setSelected.checked = getSelectedInput.checked;
            }
            else {
                const getSelectedValues = this.SetGetSelectedValues(getSelected);

                if (getSelectedValues.includes(setSelected.value)) setSelected.checked = true;
                else {
                    setSelected.checked = false;
                    this.ClearFields(setSelected);
                }
            };
        };
    };

    public Positions() {
        const getSelected = <HTMLInputElement[]>Utils.GetArrayFromClass('item-setter-get-id', this.ItemGetter);
        const getSelectedValues = this.SetGetSelectedValues(getSelected);

        const allSetPositions = <HTMLInputElement[]>Utils.GetArrayFromClass('set-position', this.Element);

        for (const setPosition of allSetPositions) {
            const setId = <HTMLInputElement>setPosition.parentElement!.getElementsByClassName('set-id')[0];
            const setIdVal = setId.value;

            if (getSelectedValues.includes(setIdVal)) {
                const positionToSet = getSelectedValues.indexOf(setIdVal);
                setPosition.value = positionToSet.toString();
            }
            else setPosition.value = '';
        };
    };

    public Width() {
        const getSelected = <HTMLInputElement[]>Utils.GetArrayFromClass('item-setter-get-id', this.ItemGetter);
        const setWidthsArray: { id: string, width: number }[] = [];

        for (const getSelect of getSelected) {
            const id = getSelect.value;
            const classList = Array.from(getSelect.parentElement!.classList);
            let itemWidthValue = 12;

            for (const className of classList) {
                if (className.substring(0, 7) === 'col-md-') {
                    itemWidthValue = parseInt(className.replace('col-md-', ''));
                };
            };

            setWidthsArray.push({ id: id, width: itemWidthValue });
        }

        const allSetIds = <HTMLInputElement[]>Utils.GetArrayFromClass('set-id', this.Element);

        for (const setWidth of setWidthsArray) {
            const setSelectedInput = <HTMLInputElement>allSetIds.find(element => element.value == setWidth.id);
            const setWidthInput = <HTMLInputElement>setSelectedInput.parentElement!.getElementsByClassName('set-width')[0];
            setWidthInput.value = setWidth.width.toString();
        }
    };

    private SetGetSelectedValues(getSelected: HTMLInputElement[]) {
        let getSelectedValues: string[] = [];

        for (const selected of getSelected) {
            getSelectedValues.push(selected.value);
        }

        return getSelectedValues;
    }

    private ClearFields(setSelectedInput: HTMLElement) {
        const setSelectedParent = setSelectedInput.parentElement!;

        const setPositions = <HTMLInputElement[]>Utils.GetArrayFromClass('set-position', setSelectedParent);
        const setWidths = <HTMLInputElement[]>Utils.GetArrayFromClass('set-width', setSelectedParent);

        if (setPositions.length > 0) setPositions[0].value = '';
        if (setWidths.length > 0) setWidths[0].value = '';
    }
}