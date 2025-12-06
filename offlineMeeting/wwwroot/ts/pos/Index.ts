import { ControlModal } from "@root/share/ControlModal";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { BackButton } from "@root//share/BackButton";
import { BlurImage } from "@root//share/BlurImage";
import { CaliculateCoinNumber } from "./CaliculateCoinNumber";
import { StockOrder } from "./StockOrder";


(async () => {
    new ControlNavbar();
    const controlModal = new ControlModal();
    const caliculateCoinNumber = new CaliculateCoinNumber();
    const stockOrder = new StockOrder();
    const backButton = new BackButton();

    controlModal.setControl();
    caliculateCoinNumber.setCoin();
    stockOrder.addOrder();
    backButton.Buck();
    BlurImage.loaded();
})();