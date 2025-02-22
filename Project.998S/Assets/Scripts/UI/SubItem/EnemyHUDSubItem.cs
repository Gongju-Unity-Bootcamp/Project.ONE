using System.Collections.Generic;

public class EnemyHUDSubItem : UISubItem
{
    private enum RawImages
    {
        EnemyRenderRawImage
    }

    private enum Images
    {
        EnemyHealthGaugeImage
    }

    private enum Texts
    {
        EnemyNameText,
        EnemyLevelText,
        EnemyLargeHealthText,
    }

    private int enemyIndex;
    private Enemy enemy;
    private CharacterID id;
    private CharacterData data;

    public override void Init()
    {
        base.Init();

        InitUIData();
        BindRawImage(typeof(RawImages));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        SetNameText(data);
        SetRenderTexture(data);

        enemy.currentHealth.BindModelEvent(UpdateHealthIndicator, this);
        enemy.level.BindModelEvent(UpdateLevelText, this);
    }

    private void InitUIData()
    {
        List<UISubItem> subItems = GetComponentInParent<EnemiesSubItem>().subItems;

        for (int index = 0; index < subItems.Count; ++index)
        {
            if (subItems[index] == GetComponent<EnemyHUDSubItem>())
            {
                enemyIndex = index;

                break;
            }
        }

        enemy = Managers.Stage.enemies[enemyIndex].GetCharacterInGameObject<Enemy>();
        id = enemy.characterId.Value;
        data = Managers.Data.Character[id];
    }

    private void SetNameText(CharacterData data)
    {
        GetText((int)Texts.EnemyNameText).text = data.Name;
    }

    private void SetRenderTexture(CharacterData data)
    {
        GetRawImage((int)RawImages.EnemyRenderRawImage).texture = Managers.Resource.LoadTexture(data.Prefab);
    }

    private void UpdateHealthIndicator(int health)
    {
        float maxHealth = enemy.maxHealth.Value;

        GetImage((int)Images.EnemyHealthGaugeImage).fillAmount = health / maxHealth;
        GetText((int)Texts.EnemyLargeHealthText).text = health.ToString();
    }

    private void UpdateLevelText(int level)
    {
        GetText((int)Texts.EnemyLevelText).text = level.ToString();
    }
}
