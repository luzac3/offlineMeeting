import { SignalR } from "@root/share/SignalR";
import { ModifiedUserSelect } from "@root/GameData/SetAvoidSelectDuplication"; 
import { StreamingControlModal } from "./StreamingControlModal";
import { StartStreaming } from "./StartStreaming";

( async () => {
    const signalR = new SignalR();
    const startStreaming = new StartStreaming();

    await signalR.activate();
    startStreaming.setStreaming(signalR);
    StreamingControlModal.setModal();
    ModifiedUserSelect.hangingVideoSetter("#listModalWindow");
})();