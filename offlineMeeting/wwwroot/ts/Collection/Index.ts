import { ControlModal } from "@root/share/ControlModal";
import { BackButton } from "@root//share/BackButton";
import { JumpDetail } from "./JumpDetail";


(async () => {
    const controlModal = new ControlModal();
    const jumpDetail = new JumpDetail();
    const backButton = new BackButton();

    controlModal.setControl();
    jumpDetail.Jump();
    backButton.Buck();
})();