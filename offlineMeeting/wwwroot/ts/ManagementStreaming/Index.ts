import { SignalR } from "@root/share/SignalR";
import { SelectEndHand } from "@root/GameData/SelectEndHand";
import { ModifiedUserSelect } from "@root/GameData/SetAvoidSelectDuplication";
import { EndHand } from "@root/GameData/EndHand"
import { ManagementStreamingControlModal } from "./ManagementStreamingControlModal";
import { ManagementVideo } from "./ManagementVideo";
import { SetMikeDevice } from "./SetMikeDevice";
import { Recording } from "./Recording"; 

( async () => {
    const signalR = new SignalR();
    const selectEndHand = new SelectEndHand();
    const endHand = new EndHand();
    const managementVideo = new ManagementVideo();
    const setMikeDevice = new SetMikeDevice();
    const recording = new Recording();

    await signalR.activate();
    selectEndHand.setHoraKindEvent();
    managementVideo.setStreamingEvent(signalR);
    endHand.setEndHandRegisterEvent(signalR);
    setMikeDevice.setGetMikeEvent();
    recording.setRecordEvent(signalR);
    ManagementStreamingControlModal.setModal();
    ModifiedUserSelect.hangingVideoSetter("#listModalWindow");
})();