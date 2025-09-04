export class ManagedVoiceVox {
    playVoiceVox = async (text: string, speaker: number = 3, ctx: AudioContext) => {
        const apiBaseUrl = process.env.NODE_ENV === "development"
            ? "http://localhost:50021"
            : "/voicevox-api";

        // audio_query生成
        const queryRes = await fetch(
            `${apiBaseUrl}/audio_query?text=${encodeURIComponent(text)}&speaker=${speaker}`,
            { method: "POST" }
        );
        const audioQuery = await queryRes.json();

        // 音声合成（WAVデータ取得）
        const synthRes = await fetch(
            `${apiBaseUrl}/synthesis?speaker=${speaker}`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(audioQuery)
            }
        );
        // arrayBufferで一度だけbodyを消費
        const arrayBuffer = await synthRes.arrayBuffer();

        if (ctx.state === "suspended") {
            await ctx.resume();
        }

        try {
            const buffer = await ctx.decodeAudioData(arrayBuffer);
            const source = ctx.createBufferSource();
            source.buffer = buffer;
            source.connect(ctx.destination);
            source.start(0);
        } catch (e) {
            console.error("decodeAudioData失敗:", e);
            alert("音声再生がブラウザによりブロックされました。再生ボタンを押してください。");
        }
    }
}