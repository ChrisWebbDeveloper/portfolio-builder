import { Utils } from './Utils';
import Sortable from 'sortablejs';
import { ItemResizer } from './ItemResizer';
import { Setter } from './Setter';

export class ItemSorter {

    private Element: HTMLElement;
    private ItemsNotDisplayed?: HTMLElement;
    private ItemsDisplayed: HTMLElement;
    private ItemSorterMsg?: HTMLElement;
    private Setter: HTMLElement;
    private SetterClass: Setter;

    constructor(itemResizerClass?: ItemResizer, setter?: HTMLElement, itemSorter: HTMLElement = <HTMLElement>document.getElementsByClassName('item-sorter')[0]) {
        this.Element = itemSorter;
        this.ItemsNotDisplayed = <HTMLElement>this.Element.getElementsByClassName('item-sorter-not-displayed')[0];
        this.ItemsDisplayed = <HTMLElement>this.Element.getElementsByClassName('item-sorter-display')[0];
        this.ItemSorterMsg = <HTMLElement>this.Element.getElementsByClassName('item-sorter-msg')[0];
        this.Setter = <HTMLElement>this.Element.getElementsByClassName('item-sorter-setter')[0];
        this.SetterClass = new Setter(this.ItemsDisplayed, this.Setter);

        const id = Utils.GenerateId();

        let sortableArray: HTMLElement[] = [];
        if (this.ItemsNotDisplayed) sortableArray.push(this.ItemsNotDisplayed);
        if (this.ItemsDisplayed) sortableArray.push(this.ItemsDisplayed);

        for (const elementToSort of sortableArray) {
            new Sortable(elementToSort, {
                group: 'sortable-group-' + id,
                animation: 150,
            });

            elementToSort.ondrop = () => {
                this.SetItemsToDisplayMsg();
                this.SetColClass();
                this.SetSetter();
                itemResizerClass?.EnableDisableResizerButtons();
            }
        };

        this.SetItemsToDisplayMsg();
        this.SetColClass();
        this.SetSetter();
    };


    public SetItemsToDisplayMsg() {
        if (this.ItemSorterMsg) {
            //Message included in count
            if (this.ItemsDisplayed.childElementCount == 1) {
                this.ItemSorterMsg.style.display = 'block';
            } else {
                this.ItemSorterMsg.style.display = 'none';
            }
        }
    };

    public SetColClass() {
        const itemDisplayBoxes = Utils.GetArrayFromClass('item-display-box');

        for (const itemDisplayBox of itemDisplayBoxes) {
            const parentClasses = itemDisplayBox.parentElement!.classList;

            if (parentClasses.contains('items-to-display')) itemDisplayBox.classList.add('col-12');
            else {
                let classArray = Array.from(itemDisplayBox.classList);

                for (const className of classArray) {
                    if (className.substring(0, 6) === 'col-12' || className.substring(0, 7) === 'col-md-') {
                        itemDisplayBox.classList.remove(className);
                    };
                };
            }
        };
    };

    //Filling in Setter table
    private SetSetter() {
        this.SetterClass.Selected();
        this.SetterClass.Positions();
    };

}