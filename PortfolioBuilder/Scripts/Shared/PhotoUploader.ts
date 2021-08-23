import { Utils } from "./Utils";

const ImageCompressor = require('image-compressor').ImageCompressor;

export class PhotoUploader {

    private Element: HTMLElement;
    private PhotoUploaderBox: HTMLElement;
    private PhotosToUpload: HTMLInputElement;
    private Gallery: HTMLElement;
    private PhotoUploaderMessage: HTMLElement;
    private PhotoUploaderButton?: HTMLElement;
    private AllowsMultipleFiles: Boolean = false;

    constructor(allowsMultipleFiles?: Boolean, photoUploader: HTMLElement = <HTMLElement>document.getElementsByClassName('photo-uploader')[0]) {
        this.Element = photoUploader;
        this.PhotoUploaderBox = <HTMLInputElement>this.Element.getElementsByClassName('photo-uploader-box')[0];
        this.PhotosToUpload = <HTMLInputElement>this.Element.getElementsByClassName('photos-to-upload')[0];
        this.Gallery = <HTMLElement>this.Element.getElementsByClassName('gallery')[0];
        this.PhotoUploaderMessage = <HTMLElement>this.Element.getElementsByClassName('photo-uploader-msg')[0];
        this.PhotoUploaderButton = <HTMLElement>this.Element.getElementsByClassName('photo-uploader-button')[0];
        if (allowsMultipleFiles) this.AllowsMultipleFiles = allowsMultipleFiles;

        this.PhotosToUpload.ondragenter = () => this.SetHover(true);
        this.PhotosToUpload.onmouseover = () => this.SetHover(true);

        this.PhotosToUpload.ondragleave = () => this.SetHover(false);
        this.PhotosToUpload.onmouseout = () => this.SetHover(false);
        this.PhotosToUpload.ondrop = () => this.SetHover(false);

        this.PhotosToUpload.oninput = () => this.ImagesPreview();

        if (this.PhotoUploaderButton) this.PhotoUploaderButton.onclick = (event) => this.UploadButtonClick(event);
    }

    private UploadButtonClick(event: Event) {
        if (this.PhotosToUpload.value.length === 0) {
            Utils.Alert('No photos have been selected for uploading');
            return false;
        }

        Utils.Confirm('Are you sure you want to add the photos above?', event);
    }

    private SetHover(hovered: boolean) {
        let bgColor: string;
        let textDecoration: string;

        if (hovered) {
            bgColor = '#92dce5' //siteMiddleBlue;
            textDecoration = 'underline';
        } else {
            bgColor = 'white';
            textDecoration = 'none';
        };

        this.PhotoUploaderBox.style.backgroundColor = bgColor;
        this.PhotoUploaderMessage!.style.textDecoration = textDecoration;
    }

    private ImagesPreview(): void {
        const fileTypes = ['jpg', 'jpeg', 'png'];
        this.Gallery.innerHTML = '';

        let displayImagesMsg: string;

        if (!this.PhotosToUpload.files || this.PhotosToUpload.files?.length == 0) {
            displayImagesMsg = 'Click here to select files to upload, or drag and drop them into this box';
        }
        else {
            if (this.PhotosToUpload.files?.length == 1) displayImagesMsg = this.PhotosToUpload.files[0].name;
            else displayImagesMsg = this.PhotosToUpload.files!.length.toString() + ' files selected';
        }

        this.PhotoUploaderMessage.innerHTML = displayImagesMsg;

        if (this.PhotosToUpload.files) {
            let filesArray = Array.from(this.PhotosToUpload.files);

            for (const file of filesArray) {
                const extension = file.name.split('.').pop()?.toLowerCase();
                const correctFileType = fileTypes.indexOf(extension!) > -1;

                if (correctFileType) {
                    const reader = new FileReader();

                    reader.onload = () => {
                        const imgResult = <string>reader.result;

                        const compressor = new ImageCompressor;
                        let compressorSettings;

                        if (this.AllowsMultipleFiles) {
                            compressorSettings = {
                                toWidth: 150,
                                toHeight: 150,
                                mode: 'strict',
                                quality: 0.1,
                            };
                        }
                        else {
                            compressorSettings = {
                                toWidth: 800,
                                toHeight: 800,
                                mode: 'strict',
                                quality: 0.1,
                            };
                        }

                        const proceedCompressedImage = (compressedSrc: string) => {
                            if (this.AllowsMultipleFiles == true) {
                                let imgPreview = `<div class='col-6 col-sm-4 col-md-3 col-lg-2'><img class='uploader-photo-display' src='${compressedSrc}' alt='${compressedSrc}' /></div>`
                                this.Gallery.innerHTML = this.Gallery.innerHTML + imgPreview;
                            }
                            else {
                                this.Gallery.innerHTML = `<div class='col-12 d-flex'><img class='photo-display-lg' src='${compressedSrc}' alt='${compressedSrc}' /></div>`
                            }
                        };

                        compressor.run(imgResult, compressorSettings, proceedCompressedImage);
                    };

                    reader.readAsDataURL(file);
                }
            }
        }
    };
}