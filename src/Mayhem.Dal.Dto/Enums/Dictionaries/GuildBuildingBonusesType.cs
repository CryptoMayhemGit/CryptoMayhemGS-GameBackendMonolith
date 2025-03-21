namespace Mayhem.Dal.Dto.Enums.Dictionaries
{
    public enum GuildBuildingBonusesType
    {                                                                       // tier 1 | tier 2 | tier 3 | tier > 3
        All = 1, // niewielka premia do wszystkich działań gracza                1%   |   2%   |   3%   | +0.1%
        DiscoveryDetection, // premia do tempa eksploracji terenu oraz wydobycia 10%  |   20%  |   30%  | +0.1%
        MechConstruction, // premia do tempa konstruowania mechów                5%   |   10%  |   15%  | +0.1%
        MechAttack, // premia do rozwalania Obcych mechami                       1%   |   2%   |   3%   | +0.1%
        MoveSpeed, // szybsze przemieszczanie się po mapie                       10%  |   15%  |   20%  | +0.1%
    }
}

