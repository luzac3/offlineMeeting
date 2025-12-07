import { ControlModal } from "@root/share/ControlModal";
import { BackButton } from "@root//share/BackButton";
import { BlurImage } from "@root//share/BlurImage";
import { CaliculateCoinNumber } from "./CaliculateCoinNumber";
import { StockOrder } from "./StockOrder";


(async () => {
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