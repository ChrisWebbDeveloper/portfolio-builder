import { Utils } from './Utils';

export class CatPortLinks {

    private Elements: Array<HTMLElement>;

    constructor(catPortLinks: Array<HTMLElement> = Utils.GetArrayFromClass('cat-port-link')) {
        this.Elements = catPortLinks;

        for (const Element of this.Elements) {
            this.ExtendLink(Element);
        }
    }

    private ExtendLink(catPortLink: HTMLElement) {
        const catPortDisplay = <HTMLElement>catPortLink.parentElement!.closest('.cat-port-display-box');

        catPortDisplay.onclick = () => {
            window.location.href = catPortLink.getAttribute('href')!;
        }
    }


}



