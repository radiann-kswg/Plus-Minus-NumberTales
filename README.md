# ±ナンバーテールズ（Plus-Minus-NumberTales）

一次創作「ナンバーテールズ」の1桁番キャラクターをモチーフにした**数字合わせパズル**（Unity 6 / WebGL）。
流れてくる数字を左右どちらかの器に足し込み、選んだキャラの番号を **＋と−の両方**で揃えると大量得点。

- 遊ぶ: https://unityroom.com/games/plus-minus-numbertales
- キャラクター設定: https://database.numbertales-radiann.net/

## 遊び方

| 要素 | 中身 |
| --- | --- |
| 目標の数字 | キャラ選択で決まる（1〜9）。器の数字が **±目標** になると得点（200）、左右で **＋目標と−目標** が同時に揃うと**全消し**（1000） |
| 器（左右） | 流れてくる数字を足していく。合計の絶対値が 10 以上になると**バースト**（器はランダム値に戻り、両色のバブルが 5 個ずつ降る） |
| バブル | 数字を入れるたびにその符号のバブルが 3〜5 個降る。目標を作ると同じ符号のバブルが消えて 1 個 50 点（連鎖は最大 3 回）。バブルが判定線に触れると**ゲームオーバー** |
| ホールド | 今の数字を取り置いて次と入れ替える。1 手につき 1 回 |
| レベル | スコア 2000 ごとに 1 上昇（上限 51 で `MAX`）。上がるほどベルトが速く、出る数字の幅が広がる。得点・レベルの実値は `MainScene` の `GameDirector` に設定されている（コードの既定値ではない） |
| No0 モード | タイトルで隠しコマンド（→↓←→↑←↓↓↑↑→←）を入力してから 000(チトセ) を選ぶと解禁。目標を作るたびに目標の数字が変わる |

## 操作

キーボード・ゲームパッド両対応（Input System）。

| 場面 | 操作 | キーボード | ゲームパッド |
| --- | --- | --- | --- |
| 共通 | 決定 | Enter / Space | A |
| タイトル | 隠しコマンド | 矢印 / WASD / テンキー | 十字キー |
| キャラ選択 | 左右 | ← → / A D / 4 6 | 十字キー左右 / LB RB |
| キャラ選択 | タイトルへ戻る | Esc / BackSpace | B |
| メイン | 器の切替 | ← → / A D / 4 6 | 十字キー左右 / LB RB |
| メイン | ハードドロップ | ↓ / S / 5 / Enter | 十字キー下 / A |
| メイン | ホールド | ↑ / W / 8 / Space / Shift | 十字キー上 / Y |
| リザルト | 共有QR切替（X ⇄ Misskey） | ← → / T / M / X | 十字キー左右 / LB RB / X / B |

## シーン構成

`TitleScene` → `CharacterSelectScene` → `MainScene` → `ResultScene` の順（`ProjectSettings/EditorBuildSettings.asset`）。
シーン間の値（目標番号・スコア・No0 解禁）は `Messerger` シングルトンで受け渡す。

## 動作環境

| | |
| --- | --- |
| エンジン | Unity **6000.3 LTS**（`ProjectSettings/ProjectVersion.txt`） |
| ビルド | **WebGL**（unityroom 向け）。`Tools > PluMi > Build WebGL` で `Builds/WebGL/` に出力（Gzip・Decompression Fallback オフ＝unityroom の要件。設定は Player Settings 側）。エディタでは `TitleScene` から Play |
| 主要パッケージ | Input System / 2D Animation / Timeline / uGUI |
| 外部ライブラリ | ZXing（`Assets/Plugins/zxing.unity.dll`、共有QR生成） |
| 遊び方動画 | タイトルの動画は `https://www.numbertales-radiann.com/drive/game_prmv.mp4` を URL 再生（`Assets/Videos/` の mp4 はビルドに含めない）。WebGL では操作前の自動再生がブラウザに拒否されることがあり、その回はスキップして次の待機へ回る |

## リポジトリの中身

| パス | 中身 |
| --- | --- |
| `Assets/Scripts/GamePart/` | メインシーン。`GameDirector`（進行・得点・レベルデザイン）/ `GameController`（入力）/ `GameObjectClasses/`（ベルト・切替・数字表示・バブル判定） |
| `Assets/Scripts/Singletons/` | `Messerger` / `SE` / `TransitionManager`（プレハブは `Assets/Prefab/Singletons/`） |
| `Assets/Scripts/` 直下 | タイトル・キャラ選択・隠しコマンド・遊び方動画 |
| `Assets/Scripts/OptionalPart/` | `TweetSystem`（X / Misskey 共有QR） |
| `Assets/Editor/` | `WebGLBuilder`（WebGL ビルドメニュー）/ `GitTools`（Unity 側で git を実行。Cowork 用） |
| `Assets/Scenes/` | 本番 4 シーン＋`Test/`（検証用） |
| `Assets/Images/` | キャラアイコン・立ち絵・数字スプライト・バブル・背景 |
| `Assets/Sounds/` | BGM（キャラごと）/ SE（対応表は `SE/SE_AudioClipList.txt`） |
| `Assets/Videos/` | 遊び方動画（タイトルで自動再生） |
| `Assets/Fonts/soria/` | soria フォント |
| `docs/roleplay/` | AIエージェント用ロールプレイ正典（`AGENTS.md` 7章） |

## セットアップ

```bash
git clone https://github.com/radiann-kswg/Plus-Minus-NumberTales.git
```

Unity Hub で開き、`TitleScene` を Play。WebGL ビルドは `Tools > PluMi > Build WebGL`（または Build Profiles）。

## ライセンス

**CC BY-NC 4.0**（権利者: 百花繚乱研究所 / ラジアン）。全文は [LICENSE](LICENSE)。
創作キャラクターの利用は創作DBサイトのガイドラインに従ってください。

サードパーティ:

- BGM / SE … 魔王魂（https://maou.audio/ 利用規約に従う）
- `Assets/Fonts/soria/` … soria（SIL Open Font License 1.1。同フォルダ内ライセンス参照）
- `Assets/Plugins/zxing.unity.dll` … ZXing.Net（Apache License 2.0）

エージェント向けの運用ルール（ブランチ・Unity MCP・ロールプレイ・README更新）は [AGENTS.md](AGENTS.md) が単一情報源です。
