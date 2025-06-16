using UnityEngine;

public class Enums : MonoBehaviour
{

}
public enum SlotSize
{
    Small = 1,
    Medium = 2,
    Large = 3,
    Custom = 4
}
public enum SensorTypes
{
    Thermal,
    Optical,
    Magnetic,
    Humidity,
    Radiation
}
public enum Ratings
{
    D,
    C,
    B,
    A,
    S,
}
public enum StructuralSubtype
{
    Pipe,
    Screw,
}
public enum MaterialTypes
{
    Plastic,
    Silicone,
    Titanium,
    CarbonFiber,
    Aluminum,
    Steel,
    Copper,
    Brass,
    Tungsten_Carbide, /* Very Hard, Radiation Resistant, Very Heavy, Silver, Earth Material */
    Lead_Titanium_Alloy, /* Dark Grey, Toxic, Very Heavy, Very Tough, Radiation Resistant (80-90%), */
    Lead, /* Very Toxic, Radiation Immune (90-99%), Very Heavy, Brittle */
    Silicone_Carbide,
    Ceramic,
    Nickel_Chromium,
    Boron_Nitride,
    Self_Healing_Polymer,
    Aerogel,
    Rubber,
    Plutonium, /* Fuel, Very Radioactive, Enriched, Silvery Grey, Volatile */
    Uranium, /* Fuel, Radioactive, Silvery Grey */
    Thorium, /* Fuel, Less Radioactive, Silvery, Common, Needs Uranium or Plutonium in conjunction to work properly */
    Tritium, /* Fuel, Found on Moon, Grey */
    Lithium

}
public enum CoolantTypes
{
    Air,
    Water,
    LiquidNitrogen,
    Vacuum
}
public enum HeatingTypes
{
    Inductive,
    ChemicalReactor,
    Infrared,
    Microwave
}
public enum FilterMedia
{
    Carbon,
    Nanomesh,
}
public enum SealantMaterial
{
    Silicone,
    Aerogel,
    Thermoplastic
}
public enum EnergyTypes
{
    Battery,
    Solar,
    Fusion,
    Nuclear,
    Chemical,
    WallOutlet,
}
public enum ComponentType
{
    Heating,
    Cooling,
    Chip,
    Sensor,
    Power,
    Sealant,
    Special,
    Structural,
    Filter,
}
public enum HeatTolerance
{
    // Represents threshold before heat penalties are applied
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh,
}
public enum HeatResistance
{
    // Represents how severe the penalties applied are
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh,
}
