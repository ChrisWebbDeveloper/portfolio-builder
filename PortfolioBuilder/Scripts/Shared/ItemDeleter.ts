import { Utils } from './Utils';
import { ItemResizer } from './ItemResizer';
import { ItemSorter } from './ItemSorter';
import { Setter } from './Setter';

export class ItemDeleter {

    private Element: HTMLElement;
    private DeleteOptionsBox: HTMLElement;
    private DeleteSelectAllButton: HTMLElement;
    private DeleteDeselectAllButton: HTMLElement;
    private DeleteButton: HTMLElement;
    private DeleteGets: HTMLInputElement[];
    private ItemDeleterBoxes: HTMLElement[];
    private ItemsToDelete: HTMLElement[];
    private ItemDeleterDisplay: HTMLElement;
    private ItemDeleterIgnore: HTMLElement;
    private PermanentDelete?: boolean;
    private Setter: HTMLElement;
    private SetterClass: Setter;

    constructor(permanentDelete?: boolean, itemSorterClass?: ItemSorter, itemResizerClass?: ItemResizer, setter?: HTMLElement, deleteCheckboxes: HTMLElement = <HTMLElement>document.getElementsByClassName('item-deleter')[0]) {
        this.Element = deleteCheckboxes;
        this.DeleteOptionsBox = <HTMLElement>this.Element.getElementsByClassName('delete-option-box')[0];
        this.DeleteSelectAllButton = <HTMLElement>this.Element.getElementsByClassName('select-all-button')[0];
        this.DeleteDeselectAllButton = <HTMLElement>this.Element.getElementsByClassName('deselect-all-button')[0];
        this.DeleteButton = <HTMLElement>this.Element.getElementsByClassName('delete-button')[0];
        this.DeleteGets = <HTMLInputElement[]>Utils.GetArrayFromClass('item-deleter-get', this.Element);
        this.ItemDeleterBoxes = Utils.GetArrayFromClass('item-deleter-box', this.Element);
        this.ItemsToDelete = Utils.GetArrayFromClass('item-to-delete', this.Element);
        this.ItemDeleterDisplay = <HTMLElement>this.Element.getElementsByClassName('item-deleter-display')[0];
        this.ItemDeleterIgnore = <HTMLElement>this.Element.getElementsByClassName('item-deleter-ignore')[0];
        this.PermanentDelete = permanentDelete;

        if (permanentDelete) this.Setter = <HTMLElement>this.Element.getElementsByClassName('item-deleter-setter')[0];
        else this.Setter = <HTMLElement>this.Element.getElementsByClassName('item-sorter-setter')[0];

        this.SetterClass = new Setter(this.ItemDeleterDisplay, this.Setter);

        Utils.Hide(this.DeleteGets);

        //Clicking the Delete Photos Button
        this.DeleteButton.onclick = (event) => {
            this.Delete(event);

            itemSorterClass?.SetColClass();
            itemSorterClass?.SetItemsToDisplayMsg();
            itemResizerClass?.EnableDisableResizerButtons();
        }

        //Clicking Delete Get directly
        for (let deleteGet of this.DeleteGets) {
            deleteGet.onclick = () => {
                this.SetDeleteBorders();
                this.SetSetter();
            };
        };

        //Clicking Item to Delete directly
        for (let itemToDelete of this.ItemsToDelete) {
            itemToDelete.onclick = () => {
                if (this.DeleteOptionsBox!.classList.contains('active')) {
                    var itemDeleterGet = <HTMLInputElement>itemToDelete.parentElement!.getElementsByClassName('item-deleter-get')![0];
                    itemDeleterGet.checked = !itemDeleterGet.checked;
                    this.SetDeleteBorders();
                    this.SetSetter();
                };
            };
        };

        //Clicking the Delete Select/Deselect All Buttons
        this.DeleteSelectAllButton!.onclick = () => this.SelectAll();
        this.DeleteDeselectAllButton!.onclick = () => this.DeselectAll();
        this.SetSetter();
    }

    //Clicking the Delete Photos Button
    private Delete(event: Event) {
        let itemsAvailableToDelete = false;

        for (let deleteGet of <HTMLInputElement[]>this.DeleteGets) {
            if (deleteGet.checked) itemsAvailableToDelete = true;
            if (itemsAvailableToDelete == true) break;
        }

        if (itemsAvailableToDelete) {
            if (this.PermanentDelete) Utils.Confirm('Are you sure you want to delete the selected items? This cannot be undone', event);
            else this.RemoveItemsFromDisplay();
        }
        else {
            event.preventDefault();

            if (this.PermanentDelete) {
                Utils.Alert('No items have been selected for deleting');
            }
            else {
                Utils.Alert('No items have been selected for removing');
            };
        }
    };

    //Setting Delete Borders based on Delete Get for image
    private SetDeleteBorders() {
        for (const deleteGet of <HTMLInputElement[]>this.DeleteGets) {
            const parentBox = deleteGet.parentElement;

            if (parentBox && deleteGet.checked) {
                if (parentBox && !parentBox!.classList.contains('delete-checked')) parentBox!.classList.add('delete-checked');
            }
            else {
                if (parentBox && parentBox!.classList.contains('delete-checked')) parentBox!.classList.remove('delete-checked');
            }
        }
    }

    private SelectAll() {
        const displayedDeleteGets = <HTMLInputElement[]>Utils.GetArrayFromClass('item-deleter-get', this.ItemDeleterDisplay);

        for (const deleteGet of displayedDeleteGets) {
            deleteGet.checked = true;
        };

        this.SetDeleteBorders();
        this.SetSetter();
    };

    private DeselectAll() {
        for (const deleteGet of this.DeleteGets) {
            deleteGet.checked = false;
        };

        this.SetDeleteBorders();
        this.SetSetter();
    };

    public Enable(includeModal?: boolean) {
        Utils.Show(this.DeleteGets);
        this.SetDeleteBorders();

        for (const itemDeleterBox of this.ItemDeleterBoxes) {
            itemDeleterBox.classList.add('delete-active');
        }

        if (includeModal) {
            for (const itemToDelete of this.ItemsToDelete) {
                itemToDelete.removeAttribute('data-toggle');
            }
        }
    }

    public Disable(includeModal?: boolean) {
        Utils.Hide(this.DeleteGets);

        for (const itemDeleterBox of this.ItemDeleterBoxes) {
            itemDeleterBox.classList.remove('delete-active');
            itemDeleterBox.classList.remove('delete-checked');
        }

        if (includeModal) {
            for (const itemToDelete of this.ItemsToDelete) {
                itemToDelete.setAttribute('data-toggle', 'modal');
            }
        }
    }

    private RemoveItemsFromDisplay() {
        for (const itemToRemove of this.ItemDeleterBoxes) {
            if (itemToRemove.parentElement == this.ItemDeleterDisplay) {
                if (itemToRemove.classList.contains('delete-checked')) {
                    this.ItemDeleterDisplay.removeChild(itemToRemove);
                    this.ItemDeleterIgnore.appendChild(itemToRemove);
                }
            }
        }

        this.DeselectAll();
    };

    //Setting Delete Set Checkboxes based on Delete Get of same id
    private SetSetter() {
        if (this.PermanentDelete) this.SetterClass.Selected(true);
        else this.SetterClass.Selected();
    }

}