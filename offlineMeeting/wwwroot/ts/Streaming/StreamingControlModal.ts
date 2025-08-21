import { ControlModal } from "@root/share/ControlModal"

export class StreamingControlModal {
    static setModal = () => {
        document.getElementById("painList")?.classList.remove("nodisplay");

        document.getElementById("painButton")?.addEventListener("click", () => {
            document.getElementById("listModalWrapper")!.classList.remove("hidden");
        });

       const listControlModal = new ControlModal();

        listControlModal.setControl();
    }
}