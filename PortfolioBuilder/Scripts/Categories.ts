import { ItemSorter } from './Shared/ItemSorter';
import { ItemSelector } from './Shared/ItemSelector';
import { ItemResizer } from './Shared/ItemResizer';
import { Utils } from './Shared/Utils';
import { ItemDeleter } from './Shared/ItemDeleter';
import { AllOptionsBox } from './Shared/AllOptionsBox';
import { CatPortLinks } from './Shared/CatPortLinks';

export class Categories {

    public static Index() {
        new CatPortLinks();
    }

    public static IndexEdit() {
        new ItemSorter();
        new ItemResizer();

        const itemDeleter = new ItemDeleter(true);
        new AllOptionsBox({
            deleteFunction: () => itemDeleter.Enable(true),
            deleteFunctionElse: () => itemDeleter.Disable(true)
        });
    }

    public static Add() {
        Categories.ItemMethods();
    }

    public static Edit() {
        Categories.ItemMethods();
    }

    public static Delete() {
        Utils.ButtonConfirmClick('Are you sure you want to delete this portfolio? This cannot be undone');
    }

    public static Details() {
        new CatPortLinks();
    }

    private static ItemMethods() {
        new ItemSelector();

        const itemResizer = new ItemResizer();
        const itemSorter = new ItemSorter(itemResizer);

        const itemDeleter = new ItemDeleter(undefined, itemSorter, itemResizer);
        new AllOptionsBox({
            deleteFunction: () => itemDeleter.Enable(true),
            deleteFunctionElse: () => itemDeleter.Disable(true)
        });
    }
}