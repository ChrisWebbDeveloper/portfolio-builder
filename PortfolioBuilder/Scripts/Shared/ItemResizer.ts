import { Utils } from './Utils';
import { Setter } from './Setter';

export class ItemResizer {

    private Element: HTMLElement;
    private ItemResizerDisplay: HTMLElement;
    private ItemResizerBoxes: HTMLElement[];
    private ItemResizerButtons: HTMLElement[];
    private Setter: HTMLElement;
    private SetterClass: Setter;

    constructor(itemResizer: HTMLElement = <HTMLElement>document.getElementsByClassName('item-resizer')[0]) {
        this.Element = itemResizer;
        this.ItemResizerDisplay = <HTMLElement>this.Element.getElementsByClassName('item-resizer-display')[0];
        this.ItemResizerBoxes = Utils.GetArrayFromClass('item-resizer-box', this.Element);
        this.ItemResizerButtons = Utils.GetArrayFromClass('item-resizer-button', this.Element);
        this.Setter = <HTMLElement>this.Element.getElementsByClassName('item-resizer-setter')[0];
        this.SetterClass = new Setter(this.ItemResizerDisplay, this.Setter);

        this.EnableDisableResizerButtons();

        for (const itemResizerButton of this.ItemResizerButtons) {
            itemResizerButton.onclick = (event) => event.preventDefault();

            itemResizerButton.onmousedown = (event) => {
                event.preventDefault();
                this.SetItemResizerButton(itemResizerButton);
            };

            this.Element.onmouseup = () => {
                this.Element.onmousemove = null;
                this.SetSetter();
            };
        };

        window.onresize = () => {
            this.EnableDisableResizerButtons();
        };

        this.SetSetter();
    };

    public EnableDisableResizerButtons() {
        const resizerIgnoreIncluded: boolean = this.Element.getElementsByClassName('item-resizer-ignore').length > 0;

        for (const itemResizerBox of this.ItemResizerBoxes) {
            // Bootstrap md width
            if (window.innerWidth >= 768) {
                if (resizerIgnoreIncluded) {
                    const itemResizerIgnore = itemResizerBox.parentElement!.closest('.item-resizer-ignore');

                    if (itemResizerIgnore) this.Disable(itemResizerBox);
                    else this.Enable(itemResizerBox);
                }
                else {
                    this.Enable(itemResizerBox);
                }
            }
            else {
                this.Disable(itemResizerBox);
            }
        };
    };

    private Enable(itemResizerBox: HTMLElement) {
        const itemResizerBtn = <HTMLElement>itemResizerBox.getElementsByClassName('item-resizer-button')[0];
        if (itemResizerBtn) itemResizerBtn.style.display = 'flex';
    };

    private Disable(itemResizerBox: HTMLElement) {
        const itemResizerBtn = <HTMLElement>itemResizerBox.getElementsByClassName('item-resizer-button')[0];
        if (itemResizerBtn) itemResizerBtn.style.display = 'none';
    };

    private SetItemResizerButton(itemResizerButton: HTMLElement) {
        const itemResizerBox = <HTMLElement>itemResizerButton.parentElement!.closest('.item-resizer-box');
        this.Element.onmousemove = (event) => this.Resize(itemResizerBox, event);
    };

    private Resize(itemResizerBox: HTMLElement, e: MouseEvent) {
        let photoWidthCol: Number;

        const xPos = e.pageX - (this.Element.offsetLeft + itemResizerBox.offsetLeft);

        photoWidthCol = Math.round((xPos / this.Element.clientWidth) * 12);

        if (photoWidthCol < 1) photoWidthCol = 1;
        if (photoWidthCol > 12) photoWidthCol = 12;

        const classArray = Array.from(itemResizerBox.classList);

        for (const className of classArray) {
            if (className.substring(0, 7) === 'col-md-') {
                itemResizerBox.classList.remove(className);
            };
        };

        itemResizerBox.classList.add('col-md-' + photoWidthCol.toString());
    };

    public SetSetter() {
        this.SetterClass.Width();
    };

};