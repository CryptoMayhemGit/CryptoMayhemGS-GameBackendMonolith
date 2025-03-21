namespace Mayhem.Dal.Dto.Enums.Dictionaries
{
    public enum BuildingBonusesType
    {                                                                              // tier 1 | tier 2 | tier 3 | tier > 3
        Wood = 1, // przyspiesza pozyskiwanie Lekkiego Drewna i Twardego Drewna         1%   |   2%   |   3%   | +0.1%
        Mining, // przyspiesza pozyskiwanie Rudy Żelaza i Rudy Tytanu                   1%   |   2%   |   3%   | +0.1%
        Construction, // szybsze stawianie innych budynków                              1%   |   2%   |   3%   | +0.1%
        MechaniumCollection, // umożliwia pozyskiwanie Mechanium                        0%   |   1%   |   2%   | +0.1%
        Attack, // produkcja broni                                                      1%   |   2%   |   3%   | +0.1%
        Cereal, // produkcja Zboża                                                      0%   |   10%  |   20%  | +0.1%
        Meat, // produkcja Mięsa                                                        0%   |   10%  |   10%  | +0.1%
    }
}
