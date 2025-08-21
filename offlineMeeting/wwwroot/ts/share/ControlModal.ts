export class ControlModal {
    setControl = () => {
        Array.from(document.getElementsByClassName("modal_parent")).forEach((element) => {
            const modalParent = element as HTMLElement;
            const modalElement = modalParent.querySelector(".modal_window") as HTMLElement;
            const modalWrapper = modalParent.querySelector(".modal_wrapper") as HTMLElement;

            modalParent.querySelector(".close_modal")?.addEventListener("click", () => {
                this.hidden(modalWrapper);
            });

            modalWrapper.addEventListener("click", (event: Event) => {
                if (modalElement && !modalElement.contains(<HTMLElement>event.target)){
                    this.hidden(modalWrapper);
                }
            });

            modalParent.querySelector(".open_modal")?.addEventListener("click", () => {
                this.show(modalWrapper);
            });
        });
    }

    private show = (modalWrapper: HTMLElement) => {
        modalWrapper?.classList.remove("nodisplay");
    }

    private hidden = (modalWrapper: HTMLElement) => {
        modalWrapper?.classList.add("nodisplay");
    }
}