# SpiritBombTest_HoNgocThai

## 1. Giới thiệu

`SpiritBombTest_HoNgocThai` là game bắn máy bay 2D dạng vertical-scrolling được làm bằng Unity `2022.3.62f2` cho bài test Unity Developer của SpiritBomb. Người chơi điều khiển máy bay chính, né đạn và tiêu diệt các đợt enemy theo wave, dùng các boost như tên lửa, khiên và máy bay hỗ trợ để vượt qua màn chơi. Project tập trung vào gameplay hoàn chỉnh, cảm giác va chạm rõ, hiệu ứng nổ, âm thanh, rung Android, UI/HUD dễ đọc và luồng chơi từ menu đến combat.

## 2. Thông tin project

- Engine: Unity `2022.3.62f2`
- Ngôn ngữ code: C#
- Target chính: Android
- Render pipeline: Built-in/Unity UI 2D
- Scene mở đầu để test: `Assets/Scenes/BootLoader.unity`
- APK build: `LogicTest_SpiritBomb_HoNgocThai.apk` ở thư mục root của project
- Save data: PlayerPrefs JSON, schema hiện tại `saveSchemaVersion = 5`

## 3. Cách mở và chạy project

1. Clone hoặc tải project về máy.
2. Mở Unity Hub.
3. Chọn Unity version `2022.3.62f2`.
4. Add project tại thư mục chứa `Assets`, `Packages`, `ProjectSettings`.
5. Mở scene `Assets/Scenes/BootLoader.unity`.
6. Bấm Play để chạy flow đầy đủ:
   - `BootLoader -> Loading -> Menu`
   - `Menu -> SelectLevel`
   - `SelectLevel -> Loading -> GamePlay`

Nếu muốn build Android:

1. Vào `File > Build Settings`.
2. Chọn platform `Android`.
3. Đảm bảo các scene trong Build Settings gồm:
   - `BootLoader`
   - `Loading`
   - `Menu`
   - `SelectLevel`
   - `GamePlay`
4. Bấm `Build` hoặc `Build And Run`.

## 4. Điều khiển

### Android

- Chạm và kéo trên màn hình để di chuyển máy bay.
- Máy bay tự bắn đạn thường sau khi vào combat.
- Bấm icon tên lửa để bật boost missile.
- Bấm icon máy bay hỗ trợ để gọi support plane.
- Bấm icon khiên để bật shield.
- Bấm pause ở góc phải để tạm dừng, retry hoặc về menu.

### Unity Editor

- Giữ chuột trái và kéo để di chuyển máy bay.
- Các nút UI dùng chuột để click.
- `Escape` được dùng ở một số scene/panel để back hoặc xử lý pause tùy trạng thái hiện tại.

## 5. Nội dung đã làm được

### Core gameplay

- Máy bay người chơi di chuyển mượt bằng touch/mouse.
- Cơ chế bắn tự động bằng bullet pool.
- Boost missile, shield và support plane.
- Enemy spawn theo wave data.
- Có va chạm giữa player, enemy, projectile, item và boss.
- Có máu player, máu boss, score, hit counter, win và lose state.
- Có save-me popup khi player thua và đủ crystal.
- Có end-game result cho thắng/thua, gold, diamond, kill rate, score và best score.

### Level và wave

- Có 3 level gameplay được cấu hình bằng ScriptableObject:
  - `Assets/Resources/Config/Gameplay/Levels/Level1.asset`
  - `Assets/Resources/Config/Gameplay/Levels/Level2.asset`
  - `Assets/Resources/Config/Gameplay/Levels/Level3.asset`
- Mỗi level có energy cost riêng để vào màn.
- Wave được cấu hình data-driven qua `LevelConfigSO`.
- Các kiểu di chuyển enemy đang hỗ trợ:
  - Đi thẳng xuống
  - Follow path
  - Formation hold
  - Formation sine
  - Formation circle
  - Formation diagonal hold
- Level 2 đã được chỉnh để wave sine rõ hơn, diagonal song song dễ nhìn hơn và đội hình vòng tròn giãn hơn.

### Enemy, boss và plane

- Enemy thường: 2 loại
  - `Enemy1` / Basic
  - `Enemy2` / Heavy
- Boss: 1 loại
  - `Boss1`
- Tổng combat type trong `EnemyCatalog`: 3 loại gồm 2 enemy thường và 1 boss.
- Plane chính: 1 cấu hình máy bay người chơi
  - `Assets/Resources/Config/Planes/Plane1.asset`
- Support plane: 1 sprite/config hỗ trợ nằm trong cùng `Plane1.asset`.
- Boost support hiện gọi 2 máy bay phụ trợ trong gameplay.

### Upgrade và progression

- Có upgrade máy bay chính bằng gold/crystal.
- Có stat Attack, Defend, Energy và chỉ số cộng thêm khi nâng cấp.
- Có upgrade item/boost:
  - Missile
  - Support/Reinforce
  - Shield
- Có hệ thống star theo level.
- Có high score theo level.
- Có tổng điểm cao nhất tính từ danh sách high score.
- Có dữ liệu mặc định cho lần chơi đầu để test nhanh trên Android.

### UI/HUD

- Menu chính có setting, upgrade plane, upgrade boost và chọn level.
- Select Level hiển thị star, level info và energy cost trên nút Fight.
- Gameplay HUD hiển thị:
  - Máu player
  - Score
  - Hit counter
  - Số lượng boost rocket/support/shield
  - Pause button
  - Boss health bar khi boss xuất hiện
- UI boost cập nhật số lượng ngay sau khi dùng bằng callback.
- Popup thiếu item trong gameplay có nút đóng.
- Settings sound/vibration dùng toggle on/off mới ở menu và pause panel.

### Game feel và polish

- Có hiệu ứng nổ enemy/boss.
- Có hiệu ứng shield, rocket, support, item pickup.
- Có âm thanh menu, gameplay, button, shield, win và game over.
- Có rung Android qua service `Vibration`.
- Có background scrolling trong gameplay.
- Có loading scene riêng giữa các luồng chính.

### Save/load

- Save chính dùng JSON trong PlayerPrefs qua `SaveController`.
- Có service tách riêng cho:
  - Inventory: gold, crystal, energy, item boost.
  - Progress: star, high score, level complete.
  - Settings: sound, vibration, control.
  - Upgrade: plane và boost level.
- Đã bỏ các field PlayerPrefs dư không còn dùng như `timeX2Coin`, `timeSaveX2Coin`, `controlType`, `totalHighScore`.

## 6. Cấu trúc project

```text
Assets/
  AdvancedPlayerPrefsWindow/      Tool xem/chỉnh PlayerPrefs trong Editor
  Animation/                      Animator Controller và AnimationClip dùng cho UI/gameplay
  Editor/                         Editor audit/tool hỗ trợ kiểm tra scene
  FX/                             Particle, material và prefab hiệu ứng
  Font/                           Font dùng cho UI
  Prefabs/                        Prefab dùng chung như GameConfigService, Enemy1, Level1
  Resources/
    Config/                       ScriptableObject config chính
      GameConfigDatabase.asset    Database gom plane, item, enemy, path, level
      Gameplay/
        EnemyCatalog.asset        Danh sách enemy/boss
        Levels/                   Level1, Level2, Level3
        PathData.asset            Path data
        PathData2.asset           Formation path data
      Items/
        ItemUpgradeConfig.asset   Config upgrade boost
      Planes/
        Plane1.asset              Config plane chính và support plane sprite
    *.prefab                      Prefab runtime load bằng Resources
  Scenes/
    BootLoader.unity              Scene đầu tiên để khởi tạo service
    Loading.unity                 Scene loading trung gian
    Menu.unity                    Main menu, setting, upgrade
    SelectLevel.unity             Chọn màn chơi
    GamePlay.unity                Gameplay combat
  Script/
    Bootstrap/                    BootLoader và lifecycle DontDestroyOnLoad
    Config/                       ScriptableObject model và GameConfigService
    Gameplay/
      Boss/                       Logic boss
      Core/                       GameManager, WaveManager, difficulty, event listener
      Enemies/                    EnemyControl và drop rules
      Items/                      Item pickup
      Map/                        Scroll map/path
      ObjectPooling/              Pool/factory runtime
      Player/                     Player plane, support plane, damage rules
      Projectiles/                Bullet, rocket, enemy projectile
    SaveSystem/                   PlayerPrefs JSON save/load
    Services/                     Service lớp giữa cho scene flow, inventory, progress, settings, upgrade
    State/                        GameState và GameSessionState
    UI/
      Common/                     Popup, settings, common panel
      Gameplay/                   Pause và gameplay HUD
      GameResult/                 Win/lose result
      Menu/                       Menu, select level, upgrade UI
    Utilities/                    Sound, vibration, constants, singleton, helpers
  Sound/                          Nhạc nền và sound effects
  TextMesh Pro/                   TextMeshPro resources
  UI/                             Sprite, texture, prefab UI
Packages/                         Unity package manifest
ProjectSettings/                  Unity project settings
LogicTest_SpiritBomb_HoNgocThai.apk
[SB] Unity Technical Test.pdf
```

## 7. Kiến trúc và quyết định kỹ thuật

- `BootLoader` là điểm vào chính để khởi tạo các service dùng chung.
- `SceneFlowService` gom luồng chuyển scene để tránh gọi `SceneManager.LoadScene` rải rác.
- `GameConfigService` đọc dữ liệu từ `GameConfigDatabaseSO` và các ScriptableObject con.
- `WaveManager` đọc `LevelConfigSO` để spawn enemy theo wave thay vì hardcode toàn bộ trong scene.
- `ObjectPooling` và `GameplayPoolFactory` đảm bảo object preload inactive, tránh lỗi projectile tự chạy khi vừa vào game.
- `GameState` giữ wrapper tương thích cho code cũ, còn một phần dữ liệu runtime được gom qua `GameSessionState`.
- Save data được tách qua service để UI/gameplay không truy cập PlayerPrefs trực tiếp quá nhiều.

## 8. Known issues / điểm còn có thể cải thiện

- Project hiện có APK ở root theo yêu cầu nộp source, chưa có thư mục `Build/` riêng.
- Chưa kèm video gameplay trong repository.
- Một số class gameplay lớn vẫn có thể tách nhỏ thêm nếu có thêm thời gian, đặc biệt `MyPlaneController`, `EnemyControl` và `GamePlayEventListener`.
- Một số asset UI/FX là asset pack đã được giữ lại vì còn dùng trong scene/prefab, nên project vẫn có nhiều sprite/prefab phục vụ giao diện.

## 9. Third-party assets, tools và credits

Các tài nguyên/tiện ích bên thứ ba còn trong project:

- DOTween/Demigiant: dùng cho tween animation trong UI/gameplay.
- TextMeshPro: dùng cho text rendering.
- AdvancedPlayerPrefsWindow: tool Editor để kiểm tra PlayerPrefs.
- GUI Pro-SurvivalClean / PackUI: dùng cho nhiều sprite/prefab UI.
- Font assets:
  - NEXON Football Gothic
  - Ubuntu
  - Electrolize
  - LiberationSans SDF từ TextMeshPro
- Sound effects/music trong `Assets/Sound` và `Assets/Sound/sound effects`.

Ghi chú: phần asset pack/font/audio nên được kiểm tra lại license gốc nếu project dùng ngoài phạm vi technical test.

## 10. AI Usage Declaration

AI tools đã được sử dụng:

- ChatGPT/Codex: hỗ trợ rà soát code, refactor có kiểm soát, debug lỗi runtime, dọn project, viết README và chuẩn bị repository.

Các phần có AI hỗ trợ đáng kể:

- Chuẩn hóa flow scene qua `SceneFlowService`.
- Rà và clean một số script UI/gameplay.
- Sửa contract object pool để object preload inactive.
- Hỗ trợ chuyển một phần config gameplay sang hướng data-driven bằng ScriptableObject.
- Hỗ trợ sửa save data PlayerPrefs và loại bỏ field dư.
- Hỗ trợ rà third-party SDK/tên template còn sót.
- Hỗ trợ viết tài liệu README này.

Các phần do người thực hiện trực tiếp kiểm tra và quyết định:

- Chọn visual, sprite, audio, UI layout và test cảm giác chơi trong Unity Editor/Android Simulator.
- Quyết định scope feature cần giữ hoặc xóa.
- Kiểm thử thủ công các luồng gameplay, menu, upgrade, select level và boost.
- Chọn APK build cuối để nộp.

Reflection:

AI giúp tăng tốc quá trình rà code, tìm reference và sửa các lỗi lặp lại trong nhiều scene. AI hữu ích nhất ở các phần clean code, kiểm tra serialized reference, chuẩn hóa service và viết tài liệu. Tuy vậy, các lỗi liên quan cảm giác chơi, UI bị che layer hoặc gameplay wave vẫn cần kiểm tra trực tiếp trong Unity vì chỉ đọc code không đủ. Tôi dùng AI như công cụ hỗ trợ kỹ thuật, còn quyết định cuối cùng dựa trên việc test project thật trong Editor và build Android.
