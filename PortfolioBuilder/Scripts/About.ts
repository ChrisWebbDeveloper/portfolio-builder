import { AutoScroller } from './Shared/AutoScroller';
import { AllOptionsBox } from './Shared/AllOptionsBox';
import { ItemSorter } from './Shared/ItemSorter';
import { ItemDeleter } from './Shared/ItemDeleter';

export class About {

    public static Index() {
        const aboutText = <HTMLElement>document.getElementById('about-text');
        new AutoScroller(aboutText);
    }

    public static Edit() {
        const itemSorter = new ItemSorter();
        const itemDeleter = new ItemDeleter(undefined, itemSorter);

        new AllOptionsBox({
            deleteFunction: () => itemDeleter.Enable(true),
            deleteFunctionElse: () => itemDeleter.Disable(true)
        });
    }

}