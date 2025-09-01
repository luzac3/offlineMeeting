import { ControlModal } from "@root/share/ControlModal";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { JumpDetail } from "./JumpDetail";
import { BackButton } from "./BackButton";


(async () => {
    new ControlNavbar();
    const controlModal = new ControlModal();
    const jumpDetail = new JumpDetail();
    const backButton = new BackButton();

    controlModal.setControl();
    jumpDetail.Jump();
    backButton.Buck();
})();