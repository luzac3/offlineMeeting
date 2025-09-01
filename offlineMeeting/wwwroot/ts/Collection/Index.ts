import { ControlModal } from "@root/share/ControlModal";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { BackButton } from "@root//share/BackButton";
import { JumpDetail } from "./JumpDetail";


(async () => {
    new ControlNavbar();
    const controlModal = new ControlModal();
    const jumpDetail = new JumpDetail();
    const backButton = new BackButton();

    controlModal.setControl();
    jumpDetail.Jump();
    backButton.Buck();
})();