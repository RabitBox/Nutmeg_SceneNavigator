# Scene Navigator
加算シーンベースのシーンナビゲーションサポートシステム

# 使い方
- Unity 6000.0.28f1 以降
## 前提
`AppCore` のような、アプリ起動から終了まで常駐するシーンを使用し、メイン部分はすべて加算シーンを使って実装する場合にのみ利用可能。

## 手順
1. 適当なAssetsフォルダ内で、右クリック -> Create -> Natmeg -> SceneConfigObject を選択し、コンフィグデータを作成。
    1. Permanentに、実行中は常駐するシーンの名前を登録
    2. SceneBundlesに、バンドル名とそれに紐づけるシーンの名前を登録
1. 適当なゲームオブジェクトに、`SceneNavigator.cs` をアタッチする。
1. Config の欄に、先程作成したコンフィグデータを設定する。
1. 適当な箇所から、`SceneNavigator.Instane.LoadSceneBundle("シーン名")`を実行する。

# ライセンス
- [zlib](https://opensource.org/license/Zlib)
