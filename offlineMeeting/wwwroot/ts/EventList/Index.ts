import { ControlModal } from "./ControlModal";
import { RegisterNewEvent } from "./RegisterNewEvent";
import { ControlObjectHandler } from "./ControlObjectHandler";
import { StartEvent } from "./StartEvent";
import { ManagementStreaming } from "./ManagementStreaming";
import { Streaming } from "./Streaming";

(() => {
    const controlModal = new ControlModal();
    const registerNewEvent = new RegisterNewEvent()
    const controlObjectHandler = new ControlObjectHandler();
    const startEvent = new StartEvent()
    const managementStreaming = new ManagementStreaming()
    const streaming = new Streaming()
    controlModal.setControl();
    registerNewEvent.setRegisterEventListner();
    controlObjectHandler.setControlObjectHandler();
    startEvent.start();
    managementStreaming.management();
    streaming.streaming();
})();