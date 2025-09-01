import { Counter } from "@root/share/Counter"
import { BackButton } from "@root//share/BackButton";
import { CreatePaypalButton } from "./CreatePaypalButton";

(() => {
    const counter = new Counter();
    const createPaypalButton = new CreatePaypalButton();
    const backButton = new BackButton();
    createPaypalButton.SetPaypalButton();
    counter.SetCounter();
    backButton.Buck();
})();