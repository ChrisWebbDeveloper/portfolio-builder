import { Utils } from './Utils';
// Although seemingly not used, it is required for modal function to work
// JQuery is not included as it breaks the modal function
import * as bootstrap from 'bootstrap';

export class ModalScroller {

    private Modals: HTMLElement[];

    constructor(modalScroller: HTMLElement = <HTMLElement>document.getElementsByClassName('modalScroller')[0]) {
        this.Modals = Utils.GetArrayFromClass('photo-modal', modalScroller);

        for (const [index, modal] of this.Modals.entries()) {
            const prevBtn = <HTMLElement>modal.getElementsByClassName('btn-prev')[0];
            const nextBtn = <HTMLElement>modal.getElementsByClassName('btn-next')[0];

            prevBtn.onclick = () => this.PressPrev(index, modal);
            nextBtn.onclick = () => this.PressNext(index, modal);
        };
    }

    private PressPrev(currentModalIndex: number, currentModal: HTMLElement) {
        // Cast as any to allow modal function, used for all modal functions
        ($(currentModal) as any).modal('hide');

        $(currentModal).on('hidden.bs.modal', () => {
            const modalCount = this.Modals.length;
            let prevModal: HTMLElement;

            if (currentModalIndex == 0) prevModal = this.Modals[modalCount - 1];
            else prevModal = this.Modals[currentModalIndex - 1];

            ($(prevModal) as any).modal('show');

            // End handler once used
            $(currentModal).off('hidden.bs.modal');
        });
    }

    private PressNext(currentModalIndex: number, currentModal: HTMLElement) {
        ($(currentModal) as any).modal('hide');

        $(currentModal).on('hidden.bs.modal', () => {
            const modalCount = this.Modals.length;
            let nextModal: HTMLElement;

            if (currentModalIndex == modalCount - 1) nextModal = this.Modals[0];
            else nextModal = this.Modals[currentModalIndex + 1];

            ($(nextModal) as any).modal('show');

            // End handler once used
            $(currentModal).off('hidden.bs.modal');
        });
    }
}