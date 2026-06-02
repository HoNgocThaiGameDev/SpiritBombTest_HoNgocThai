# SpiritBombTest_HoNgocThai

Đây là bài làm technical test Unity Developer của SpiritBomb. Game là dạng bắn máy bay 2D cuộn dọc, làm bằng Unity `2022.3.62f2` và build chính cho Android. Người chơi kéo máy bay để né đạn, bắn enemy theo từng wave, dùng thêm tên lửa, khiên và máy bay hỗ trợ để qua màn.

Mục tiêu của mình là làm một bản nhỏ nhưng chơi được trọn flow: mở game, vào menu, chọn level, chơi combat, thắng/thua, lưu tiến trình và có cảm giác va chạm/hiệu ứng/âm thanh tương đối rõ.

## Thông Tin Cơ Bản

- Unity version: `2022.3.62f2`
- Platform chính: Android
- Code: C#
- Scene bắt đầu để test: `Assets/Scenes/BootLoader.unity`
- File APK đã build: `LogicTest_SpiritBomb_HoNgocThai.apk`
- Save data: PlayerPrefs dạng JSON, schema hiện tại là `5`

## Cách Chạy Project

Mở project bằng Unity Hub với đúng version `2022.3.62f2`. Sau khi Unity import xong, mở scene:

```text
Assets/Scenes/BootLoader.unity
```

Bấm Play từ scene này để chạy đúng luồng game:

```text
BootLoader -> Loading -> Menu -> SelectLevel -> Loading -> GamePlay
```

Nếu build Android, vào `File > Build Settings`, chọn Android và kiểm tra Build Settings có đủ các scene:

```text
BootLoader
Loading
Menu
SelectLevel
GamePlay
```

Sau đó có thể dùng `Build` hoặc `Build And Run`.

## Điều Khiển

Trên điện thoại, chạm và kéo để di chuyển máy bay. Máy bay sẽ tự bắn sau khi bắt đầu màn chơi.

Các nút boost nằm bên phải màn hình gameplay:

- Tên lửa: bật missile boost.
- Máy bay hỗ trợ: gọi 2 support plane.
- Khiên: bật shield bảo vệ.
- Pause: tạm dừng, retry hoặc về menu.

Khi chạy trong Unity Editor có thể giữ chuột trái và kéo để giả lập thao tác touch.

## Những Phần Đã Làm

### Gameplay Chính

- Máy bay chính di chuyển bằng touch/mouse.
- Bắn đạn tự động bằng object pool.
- Enemy xuất hiện theo wave.
- Có va chạm giữa player, enemy, đạn, item và boss.
- Có máu player, thanh máu boss, score, hit counter.
- Có trạng thái thắng, thua và popup kết quả cuối màn.
- Có popup save me khi thua và còn đủ crystal.
- Có item/boost trong trận: missile, shield, support.

### Level

Hiện project có 3 level gameplay thật, cấu hình bằng ScriptableObject:

```text
Assets/Resources/Config/Gameplay/Levels/Level1.asset
Assets/Resources/Config/Gameplay/Levels/Level2.asset
Assets/Resources/Config/Gameplay/Levels/Level3.asset
```

Mỗi level có energy cost riêng. Khi chọn level, nút Fight sẽ hiển thị số energy cần dùng. Nếu không đủ energy thì không cho vào màn.

Wave trong level được đọc từ `LevelConfigSO`, không đặt cứng hết trong scene. Các kiểu movement đang dùng gồm đi thẳng, follow path, giữ đội hình, sine, vòng tròn và đường chéo. Level 2 đã được chỉnh lại để các wave dễ nhìn hơn, nhất là wave sine và đội hình vòng tròn.

### Enemy, Boss Và Plane

Trong `EnemyCatalog` hiện có:

- 2 enemy thường:
  - `Enemy1` / Basic
  - `Enemy2` / Heavy
- 1 boss:
  - `Boss1`

Về phía người chơi:

- 1 plane chính: `Plane1`
- 1 sprite support plane nằm chung trong config `Plane1.asset`
- Boost support sẽ gọi 2 máy bay nhỏ hỗ trợ bắn

Config chính nằm ở:

```text
Assets/Resources/Config/GameConfigDatabase.asset
Assets/Resources/Config/Planes/Plane1.asset
Assets/Resources/Config/Gameplay/EnemyCatalog.asset
```

### Upgrade Và Lưu Tiến Trình

Game có upgrade máy bay và upgrade boost. Máy bay có các chỉ số attack, defend, energy và tốc độ bắn. Boost có upgrade riêng cho missile, support và shield.

Dữ liệu lưu gồm:

- Gold, crystal, energy
- Số lượng rocket, shield, support
- Level đã hoàn thành
- Star từng level
- High score từng level
- Level upgrade của plane và boost
- Setting sound/vibration

Phần save đã được dọn lại để bỏ các field không dùng nữa như `timeX2Coin`, `timeSaveX2Coin`, `controlType` và `totalHighScore`.

### UI Và Âm Thanh

Các màn chính đã có UI:

- Menu
- Setting popup
- Upgrade plane
- Upgrade boost
- Select level
- Gameplay HUD
- Pause popup
- Win/Lose result

Gameplay HUD có máu player, score, hit, boost count và boss HP khi boss xuất hiện. Số lượng boost được cập nhật ngay sau khi dùng.

Âm thanh đã gắn cho menu, gameplay, click button, shield, win và game over. Android vibration cũng đã thêm qua script `Vibration`, dùng theo setting của người chơi.

## Cấu Trúc Project

```text
Assets/
  AdvancedPlayerPrefsWindow/      Tool để xem/chỉnh PlayerPrefs trong Editor
  Animation/                      AnimationClip và Animator Controller
  Editor/                         Một số tool audit scene dùng trong quá trình dọn project
  FX/                             Hiệu ứng nổ, shield, particle, material
  Font/                           Font dùng cho UI
  Prefabs/                        Prefab dùng chung
  Resources/
    Config/                       Toàn bộ config ScriptableObject chính
      GameConfigDatabase.asset
      Gameplay/
        EnemyCatalog.asset
        Levels/
        PathData.asset
        PathData2.asset
      Items/
      Planes/
    *.prefab                      Prefab runtime load qua Resources
  Scenes/
    BootLoader.unity
    Loading.unity
    Menu.unity
    SelectLevel.unity
    GamePlay.unity
  Script/
    Bootstrap/                    Boot flow và lifecycle
    Config/                       ScriptableObject config + GameConfigService
    Gameplay/
      Boss/
      Core/
      Enemies/
      Items/
      Map/
      ObjectPooling/
      Player/
      Projectiles/
    SaveSystem/                   Save/load PlayerPrefs JSON
    Services/                     Service cho inventory, progress, setting, scene flow
    State/                        GameState và GameSessionState
    UI/                           UI Menu, Gameplay, Result, Common
    Utilities/                    Sound, vibration, constants, helper
  Sound/                          Music và sound effects
  TextMesh Pro/
  UI/                             Sprite, texture, prefab UI
Packages/
ProjectSettings/
LogicTest_SpiritBomb_HoNgocThai.apk
[SB] Unity Technical Test.pdf
```

## Third-party Assets / Tools

Project có dùng một số asset/tool có sẵn:

- DOTween/Demigiant cho tween animation.
- TextMeshPro cho text.
- AdvancedPlayerPrefsWindow để kiểm tra PlayerPrefs trong Editor.
- GUI Pro-SurvivalClean / PackUI cho nhiều sprite và prefab UI.
- Một số font như NEXON Football Gothic, Ubuntu, Electrolize và LiberationSans SDF.
- Một số sound effects/music trong `Assets/Sound`.

Các asset này được dùng cho phạm vi technical test. Nếu dùng project cho mục đích khác thì nên kiểm tra lại license gốc của từng asset/audio/font.

## AI Usage Declaration

Mình có sử dụng ChatGPT/Codex trong quá trình làm bài.

Các phần có AI hỗ trợ:

- Rà code và tìm lỗi logic trong gameplay/UI.
- Gợi ý và hỗ trợ refactor một số phần như scene flow, object pool, save data và config ScriptableObject.
- Dùng GPT để gen ảnh/sprite, sau đó dùng Photoshop để xóa phông và điều chỉnh lại kích thước cho phù hợp với game.
- Dùng Lyria AI của Google để gen một số file âm thanh cho trò chơi.
- Hỗ trợ viết README.

Các phần mình trực tiếp quyết định và test:

- Lên plan để gamefeel của trò chơi tốt hơn.
- Test các flow chính trong Unity: boot, menu, select level, gameplay, win/lose, upgrade và boost.
- Điều chỉnh wave, boss HP, energy cost, vibration, sound và save data.
- Build APK cuối để nộp.

Mình xem AI như một QA làm việc cùng mình trong quá trình làm bài. AI hỗ trợ mình rà lại logic, tìm edge case, nhắc các lỗi dễ sót và gợi ý hướng refactor khi project có nhiều scene/reference cần kiểm tra. Tuy vậy phần lớn task vẫn do mình xử lý trực tiếp, đặc biệt là thiết kế gameplay, chỉnh gamefeel, quyết định UI/flow và test lại cảm giác chơi trong Unity. Quyết định cuối cùng vẫn dựa trên việc mình chạy project thật và chỉnh lại theo trải nghiệm khi chơi.
