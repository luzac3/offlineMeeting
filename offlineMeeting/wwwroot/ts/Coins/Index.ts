import { ControlNavbar } from "@root/share/ControlNavbar";
import { Counter } from "@root/share/Counter"
import { CreatePaypalButton } from "./CreatePaypalButton";
import { BackButton } from "@root//share/BackButton";
import { GiveCoins } from "./GiveCoins";

(() => {
    new ControlNavbar();
    const counter = new Counter();
    const createPaypalButton = new CreatePaypalButton();
    const backButton = new BackButton();
    const giveCoins = new GiveCoins();
    createPaypalButton.SetPaypalButton();
    counter.SetCounter();
    backButton.Buck();
    giveCoins.setCoinsData();
})();