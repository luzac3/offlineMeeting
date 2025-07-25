import { ControlModal } from "@root/share/ControlModal"

export class EventControlModal {
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

        const videoTestModal = new ControlModal(
            "#showVideoTestButton",
            "videoTest",
        );

        const showPointModal = new ControlModal(
            "#showPointButton",
            "showPoint",
        );

        const videoRegisterModal = new ControlModal(
            "#videoRegisterButton",
            "videoRegister",
        );

        listControlModal.setControl();
        endHandControlModal.setControl();
        videoTestModal.setControl();
        showPointModal.setControl();
        videoRegisterModal.setControl();
    }
}