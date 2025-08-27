import { ManagedVoiceVox } from "@root/share/ManagedVoiceVox";

export class AudioStart {
    Play = (ctx: AudioContext) => {
        document.addEventListener('DOMContentLoaded', () => {
            // 再生ボタンのイベント
            document.getElementById('play-audio')?.addEventListener('click', async () => {
                const audio = new Audio('/music/activateAudio.wav');
                audio.play();

                // AudioContextのアンロック（無音再生）
                const source = ctx.createBufferSource();
                source.buffer = ctx.createBuffer(1, 1, ctx.sampleRate);
                source.connect(ctx.destination);
                source.start(0);
            });
        });
    }
}