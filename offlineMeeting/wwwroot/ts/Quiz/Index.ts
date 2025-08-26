import { ControlNavbar } from "@root/share/ControlNavbar";
import { SendCanvasImage } from "./SendCanvasImage";
import { Canvas } from "./Canvas";

(() => {
    new ControlNavbar();
    new Canvas()
    const sendCanvasImage = new SendCanvasImage();
    sendCanvasImage.SendCanvas();
})();