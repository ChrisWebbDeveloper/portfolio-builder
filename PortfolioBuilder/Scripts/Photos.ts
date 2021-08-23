import { ModalScroller } from './Shared/ModalScroller';
import { PhotoUploader } from './Shared/PhotoUploader';
import { AllOptionsBox } from './Shared/AllOptionsBox';
import { ItemDeleter } from './Shared/ItemDeleter';
import { Utils } from './Shared/Utils';

export class Photos {

    public static Index() {
        new PhotoUploader(true);
        const itemDeleter = new ItemDeleter(true);

        new AllOptionsBox({
            deleteFunction: () => itemDeleter.Enable(true),
            deleteFunctionElse: () => itemDeleter.Disable(true)
        });

        new ModalScroller();

        //Clicking the Add Single Photo Button
        const addSinglePhotoButton = document.getElementById('add-single-photo-button')!;
        Utils.ButtonConfirmClick('Leaving the current page. Any unsaved changes will be lost.', addSinglePhotoButton);
    }

    public static Add() {
        new PhotoUploader();
    }

    public static Edit() {
        Utils.ButtonConfirmClick('Are you sure you want to save the current changes?');
    }

    public static Delete() {
        Utils.ButtonConfirmClick('Are you sure you want to delete this photo? This cannot be undone');
    }

}