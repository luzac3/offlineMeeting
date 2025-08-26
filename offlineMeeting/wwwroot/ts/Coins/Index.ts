import { ControlNavbar } from "@root/share/ControlNavbar";
import { Counter } from "@root/share/Counter"
import { CreatePaypalButton } from "./CreatePaypalButton";

(() => {
    new ControlNavbar();
    const counter = new Counter();
    const createPaypalButton = new CreatePaypalButton();
    createPaypalButton.SetPaypalButton();
    counter.SetCounter();
})();