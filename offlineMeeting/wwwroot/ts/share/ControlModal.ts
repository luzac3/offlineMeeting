import { SetEventListner } from "./SetEventListner"

export class ControlModal {
    private modalKind: string;
    private openModalElementSelector: string;

    constructor(openModalElementSelector: string, modalKind: string = "") {
        this.modalKind = modalKind;
        this.openModalElementSelector = openModalElementSelector;
    }

    setControl = () => {
        Array.from(document.getElementsByClassName("modalWindow")).forEach((element) => {
            const parentElement = element as HTMLElement;

            SetEventListner.setEvent(
                parentElement,
                "click",
                ".closeModal",
                () => {
                    this.hidden();
                }
            )

            // 外をクリックしたときのクローズイベント
            SetEventListner.setEvent(
                parentElement,
                "click",
                "#" + this.modalKind + "ModalWrapper",
                (event: Event) => {
                    (<HTMLButtonElement>event.target)
                        .closest("#" + this.modalKind + "ModalWindow") ?? this.hidden();
                }
            );

            SetEventListner.setEvent(
                parentElement,
                "click",
                this.openModalElementSelector,
                (event: Event) => {
                    this.show();
                }
            );
        });
    }

    private show = () => {
        document.getElementById(this.modalKind + "ModalWrapper")!.classList.remove("hidden");
    }

    private hidden = () => {
        console.log(this.modalKind);
        console.log(document.getElementById(this.modalKind + "ModalWrapper"));
        document.getElementById(this.modalKind + "ModalWrapper")!.classList.add("hidden");
    }
}