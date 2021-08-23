import $ from 'jquery';
import 'slick-carousel';

export class Carousel {

    private Element: JQuery<HTMLElement>;

    constructor(carousel: JQuery<HTMLElement> = $('.carousel')) {
        this.Element = carousel;

        this.Element.slick({
            lazyLoad: 'ondemand',
            slidesToShow: 1,
            slidesToScroll: 1,
            autoplay: true,
            autoplaySpeed: 3000,
            arrows: true,
            dots: true,
            pauseOnHover: false
        });
    }

}