import { ControlModal } from "@root/share/ControlModal"

export class ManagementStreamingControlModal {
    static setModal = () => {
        document.getElementById("painList")?.classList.remove("nodisplay");

        document.getElementById("painButton")?.addEventListener("click", () => {
            document.getElementById("listModalWrapper")!.classList.remove("hidden");
        });

       const listControlModal = new ControlModal(
           "#painButton",
            "list",
        );

        const endHandControlModal = new ControlModal(
            "#listEndHandButton",
            "endHand",
        );

        listControlModal.setControl();
        endHandControlModal.setControl();
    }
}