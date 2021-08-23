export class AutoScroller {

    private static SupportsNativeSmoothScroll = 'scrollBehavior' in document.documentElement.style;
    private Element: HTMLElement;
    private TextToCompare: HTMLElement;
    private Scrolling = setInterval(() => {}, 0);
    private ScrollPause = false;

    constructor(textToCompare: HTMLElement, autoScroller: HTMLElement = <HTMLElement>document.getElementsByClassName('auto-scroller')[0]) {
        this.Element = autoScroller;
        this.TextToCompare = textToCompare;

        if (AutoScroller.SupportsNativeSmoothScroll) {
            this.Element.scrollTo(0, (autoScroller.scrollHeight / 3));
            this.SetAboutImgsHt();
            this.SetScrolling();

            window.onresize = () => {
                this.SetAboutImgsHt();
                this.SetScrolling();
            };

            autoScroller.onmouseenter = () => {
                this.ScrollPause = true;
                this.SetScrolling();
            };

            autoScroller.onmouseleave = () => {
                this.ScrollPause = false;
                this.SetScrolling();
            };

            this.Element.onscroll = () => this.SetScrolling();
        };
    }

    private SetAboutImgsHt() {
        const aboutImgsInnerHtml = this.Element.innerHTML;

        // Bootstrap md width
        if (window.innerWidth >= 768) {
            let textToCompareHt = this.TextToCompare.offsetHeight;
            const vh = 60;
            let currentVh = (document.documentElement.clientHeight * vh) / 100;

            if (textToCompareHt > currentVh) this.Element.style.maxHeight = textToCompareHt.toString() + 'px';
            else this.Element.style.maxHeight = vh + 'vh';

            this.Element.innerHTML = aboutImgsInnerHtml;

            //Include some padding
            if (textToCompareHt < this.Element.scrollHeight) {
                //If original images have not been duplicated already
                if (aboutImgsInnerHtml == this.Element.innerHTML) {
                    // Clone it for top and bottom, use middle as main
                    this.Element.innerHTML = this.Element.innerHTML + this.Element.innerHTML + this.Element.innerHTML;
                }
            }
            else {
                this.Element.innerHTML = aboutImgsInnerHtml;
            }
        }
        else {
            this.Element.style.maxHeight = '100%';
            this.Element.innerHTML = aboutImgsInnerHtml;
        }
    }

    private SetScrolling() {
        if (this.ScrollPause) {
            clearInterval(this.Scrolling);
        } else {
            clearInterval(this.Scrolling);
            this.Scrolling = setInterval(() => {
                this.Element.scrollBy(0, 2)
            }, 15);
        }

        // If scrolling up past first pic, move down to middle
        if (this.Element.scrollTop < ((this.Element.scrollHeight / 3) - this.Element.offsetHeight)) {
            this.Element.scrollTo(0, (((this.Element.scrollHeight / 3) * 2) - this.Element.offsetHeight));
        }
        // If scroll down past last pic, move back up to middle
        else if (this.Element.scrollTop > (((this.Element.scrollHeight / 3) * 2))) {
            this.Element.scrollTo(0, (this.Element.scrollHeight / 3));
        };
    };
}