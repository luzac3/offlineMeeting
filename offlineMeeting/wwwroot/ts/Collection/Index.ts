import { ControlModal } from "@root/share/ControlModal";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { JumpDetail } from "./JumpDetail";


(async () => {
    new ControlNavbar();
    const controlModal = new ControlModal();
    const jumpDetail = new JumpDetail();

    controlModal.setControl();
    jumpDetail.Jump();
})();