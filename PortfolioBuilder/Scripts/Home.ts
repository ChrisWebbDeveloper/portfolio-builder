import { Carousel } from './Shared/Carousel';
import { ItemSorter } from './Shared/ItemSorter';
import { ItemDeleter } from './Shared/ItemDeleter';
import { AllOptionsBox } from './Shared/AllOptionsBox';

export class Home {

    public static Index() {
        new Carousel();
    }

    public static Edit() {
        const noOfOptionsBoxes = document.getElementsByClassName('all-options-box').length;

        for (let i = 0; i < noOfOptionsBoxes; i++) {
            const allOptionsBox = <HTMLElement>document.getElementsByClassName('all-options-box')[i];
            const itemSorter = <HTMLElement>document.getElementsByClassName('item-sorter')[i];
            const itemSetter = <HTMLElement>document.getElementsByClassName('item-sorter-setter')[i];
            const itemDeleter = <HTMLElement>document.getElementsByClassName('item-deleter')[i];

            const itemSorterClass = new ItemSorter(undefined, itemSetter, itemSorter);

            const photosItemDeleterClass = new ItemDeleter(undefined, itemSorterClass, undefined, itemSetter, itemDeleter);
            new AllOptionsBox({
                deleteFunction: () => photosItemDeleterClass.Enable(true),
                deleteFunctionElse: () => photosItemDeleterClass.Disable(true)
            }, allOptionsBox);
        }
    }

}