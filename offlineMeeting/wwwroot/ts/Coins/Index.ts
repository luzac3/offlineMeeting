import { Counter } from "@root/share/Counter"
import { CreatePaypalButton } from "./CreatePaypalButton";

(() => {
    const counter = new Counter();
    const createPaypalButton = new CreatePaypalButton();
    createPaypalButton.SetPaypalButton();
    counter.SetCounter();
})();