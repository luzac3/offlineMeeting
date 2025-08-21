import { ControlModal } from "@root/share/ControlModal"

export class ManagementStreamingControlModal {
    static setModal = () => {
        document.getElementById("painList")?.classList.remove("nodisplay");

        document.getElementById("painButton")?.addEventListener("click", () => {
            document.getElementById("listModalWrapper")!.classList.remove("hidden");
        });

       const listControlModal = new ControlModal();

        const endHandControlModal = new ControlModal();

        listControlModal.setControl();
        endHandControlModal.setControl();
    }
}