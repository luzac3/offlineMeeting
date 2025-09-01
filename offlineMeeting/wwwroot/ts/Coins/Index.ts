import { ControlNavbar } from "@root/share/ControlNavbar";
import { Counter } from "@root/share/Counter"
import { CreatePaypalButton } from "./CreatePaypalButton";
import { BackButton } from "@root//share/BackButton";

(() => {
    new ControlNavbar();
    const counter = new Counter();
    const createPaypalButton = new CreatePaypalButton();
    const backButton = new BackButton();
    createPaypalButton.SetPaypalButton();
    counter.SetCounter();
    backButton.Buck();
})();