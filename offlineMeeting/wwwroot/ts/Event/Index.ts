import { SignalR } from "@root/share/SignalR";
import { SelectEndHand } from "@root/GameData/SelectEndHand";
import { ModifiedUserSelect } from "@root/GameData/SetAvoidSelectDuplication";
import { RegisterEndGame } from "@root/GameData/RegisterEndGame";
import { ShowPoint } from "@root/GameData/ShowPoint";
import { RecordVideo } from "@root/GameData/RecordVideo";
import { EndHand } from "@root/GameData/EndHand"
import { VideoControll } from "./VideoControll";
import { StartGame } from "./StartGame";
import { EventControlModal } from "./EventControlModal";
import { RegisterVideo } from "./RegisterVideo";
import { VideoTestControll } from "./VideoTestControll";
import { GetCamera } from "./GetCamera";

(async () => {
    const signalR = new SignalR();
    const selectEndHand = new SelectEndHand();
    const videoControll = new VideoControll();
    const videoTestControll = new VideoTestControll();
    const registerVideo = new RegisterVideo();
    const startGame = new StartGame();
    const endHand = new EndHand();
    const registerEndGame = new RegisterEndGame();
    const showPoint = new ShowPoint();
    const getCamera = new GetCamera();
    const recordVideo = new RecordVideo()

    // 終了処理を挟み、コンストラクタプロパティを終了処理から変更できるようにする

    await signalR.activate();
    videoControll.controlVideoEvent(signalR);
    selectEndHand.setHoraKindEvent();
    startGame.setStartEvent();
    endHand.setEndHandRegisterEvent(signalR);
    videoTestControll.setTestVideoEvent();
    registerVideo.setRegisterVideoEvent();
    registerEndGame.setGameEndEvent();
    showPoint.setStartEvent();
    getCamera.setGetCamerEvent();
    recordVideo.setRecordEvent(signalR);
    EventControlModal.setModal();
    ModifiedUserSelect.setAvoidDuplication();
    ModifiedUserSelect.hangingVideoSetter("#listModalWindow");
})();