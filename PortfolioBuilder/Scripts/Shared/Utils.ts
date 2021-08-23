import * as bootstrap from 'bootstrap';

export class Utils {

    public static GetArrayFromClass(className: string, rootToSearch: Document | HTMLElement | Element = document): HTMLElement[] | HTMLInputElement[] {
        return Array.from(<HTMLCollectionOf<HTMLElement>>rootToSearch.getElementsByClassName(className));
    };

    public static Show(elementsToShow: HTMLElement[] | HTMLInputElement[]) {
        for (const item of elementsToShow) {
            item.classList.add('active');
            item.style.display = 'block';
        }
    }

    public static Hide(elementsToHide: HTMLElement[] | HTMLInputElement[]) {
        for (const item of elementsToHide) {
            item.classList.remove('active');
            item.style.display = 'none';
        }
    }

    public static ShowHide(elementsToShowHide: HTMLElement[] | HTMLInputElement[]) {
        for (const item of elementsToShowHide) {
            if (item.style.display === 'none') {
                item.classList.add('active');
                item.style.display = 'block';
            } else {
                item.classList.remove('active');
                item.style.display = 'none';
            }
        };
    }

    public static ButtonConfirmClick(message: string, btnToClick: HTMLElement = <HTMLInputElement>document.getElementsByClassName('submit-button')[0]) {
        btnToClick.onclick = (event) => {
            Utils.Confirm(message, event);
        };
    }

    public static Alert(message: string) {
        const modal = document.getElementsByClassName('alert-modal')[0];
        modal.getElementsByClassName('modal-body')[0].innerHTML = message;

        ($('.alert-modal') as any).modal();

        const submitBtn = <HTMLElement>modal.getElementsByClassName('alert-modal-submit')[0];

        submitBtn.onclick = () => {
            console.log("hello");
        };
    }

    public static Confirm(message: string, event: Event) {
        event.preventDefault();

        const modal = document.getElementsByClassName('confirm-modal')[0];
        modal.getElementsByClassName('modal-body')[0].innerHTML = message;

        ($('.confirm-modal') as any).modal();

        const submitBtn = <HTMLElement>modal.getElementsByClassName('confirm-modal-submit')[0];

        submitBtn.onclick = () => {
            const btn = <HTMLElement>event.target;
            const form = btn.closest('form');

            if (form) form.submit();
        };
    }

    public static GenerateId() {
        return Math.floor(Math.random() * 10000000000).toString();
    }

}