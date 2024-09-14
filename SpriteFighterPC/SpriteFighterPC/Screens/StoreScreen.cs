using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    class StoreScreen : GameScreen
    {
        NextLevelEvent m_nextLevelEvent;
        PlayerShip m_playerShip;
        BuyHardwareEvent m_buyHardwareEvent;

        HardwareMenuComponent m_selectedComponent = null;

        HardwareMenuComponent m_mcHealth;
        HardwareMenuComponent m_mcSpreadCannon;
        HardwareMenuComponent m_mcIonCannon;
        HardwareMenuComponent m_mcGravityWell;
        HardwareMenuComponent m_mcShield;
        HardwareMenuComponent m_mcStasisRay;

        MenuComponent m_mcNoCredits;
        MenuComponent m_mcPurchased;
        MenuComponent m_mcCredits;
        MenuComponent m_mcShipLife;

        Vector2 titleFontPos = new Vector2(240, 25);
        Vector2 subTextFontPos = new Vector2(240, 75);
        Vector2 healthFontPos = new Vector2(240, 300);
        Vector2 ionCannonFontPos = new Vector2(240, 350);
        Vector2 spreadCannonFontPos = new Vector2(240, 400);
        Vector2 gravityWellFontPos = new Vector2(240, 450);
        Vector2 shieldFontPos = new Vector2(240, 500);
        Vector2 stasisRayFontPos = new Vector2(240, 550);

        Vector2 creditsFontPos = new Vector2(240, 200);
        Vector2 shipLifeFontPos = new Vector2(240, 150);

        Vector2 insufFontPos = new Vector2(240, 600);
        Vector2 buyFontPos = new Vector2(240, 650);
        Vector2 backFontPos = new Vector2(240, 750);

        public StoreScreen(NextLevelEvent nextLevelEvent, PlayerShip playerShip, BuyHardwareEvent buyHardwareEvent, SpriteBatch sb, SpriteFont sf)
            : base(sb, sf)
        {
            m_nextLevelEvent = nextLevelEvent;
            m_playerShip = playerShip;
            m_buyHardwareEvent = buyHardwareEvent;

            MenuComponent mcTitle = new MenuComponent("SuperNova Hardware", titleFontPos, 1.5f, Color.Orange);
            mcTitle._origin = sf.MeasureString(mcTitle._text) / 2;

            MenuComponent mcSubText = new MenuComponent("Upgrade Your Guns!", subTextFontPos, 1.0f, Color.Orange);
            mcSubText._origin = sf.MeasureString(mcSubText._text) / 2;

            m_mcHealth = new HardwareMenuComponent(PowerUp.HealthPack, HealthPack.HEALTH_PACK_COST, "Repairs (+100) -- ", healthFontPos);
            m_mcHealth._text += m_mcHealth._cost;
            m_mcHealth._origin = sf.MeasureString(m_mcHealth._text) / 2;
            m_mcHealth.Selected += HealthPackSelected;

            m_mcIonCannon = new HardwareMenuComponent(PowerUp.IonCannon, IonCannonUpgrade.ION_CANNON_UPGRADE_COST, "Ion Cannon -- ", ionCannonFontPos);
            m_mcIonCannon._text += m_mcIonCannon._cost;
            m_mcIonCannon._origin = sf.MeasureString(m_mcIonCannon._text) / 2;
            m_mcIonCannon.Selected += IonCannonSelected;

            m_mcSpreadCannon = new HardwareMenuComponent( PowerUp.SpreadCannon, SpreadCannonUpgrade.SPREAD_CANNON_UPGRADE_COST, "Spread Cannon -- ", spreadCannonFontPos);
            m_mcSpreadCannon._text += m_mcSpreadCannon._cost;
            m_mcSpreadCannon._origin = sf.MeasureString(m_mcSpreadCannon._text) / 2;
            m_mcSpreadCannon.Selected += SpreadCannonSelected;

            m_mcGravityWell = new HardwareMenuComponent(PowerUp.GravityWell, GravityWellUpgrade.GRAVITY_WELL_UPGRADE_COST, "Gravity Well -- ", gravityWellFontPos);
            m_mcGravityWell._text += m_mcGravityWell._cost;
            m_mcGravityWell._origin = sf.MeasureString(m_mcGravityWell._text) / 2;
            m_mcGravityWell.Selected += GravityWellSelected;

            m_mcShield = new HardwareMenuComponent(PowerUp.Shield, ShieldUpgrade.SHIELD_COST, "Shield -- ", shieldFontPos);
            m_mcShield._text += m_mcShield._cost;
            m_mcShield._origin = sf.MeasureString(m_mcShield._text) / 2;
            m_mcShield.Selected += ShieldSelected;

            m_mcStasisRay = new HardwareMenuComponent(PowerUp.StasisRay, StasisRayUpgrade.STASIS_RAY_UPGRADE_COST, "Stasis Ray -- ", stasisRayFontPos);
            m_mcStasisRay._text += m_mcStasisRay._cost;
            m_mcStasisRay._origin = sf.MeasureString(m_mcStasisRay._text) / 2;
            m_mcStasisRay.Selected += StasisRaySelected;

            MenuComponent mcBuy = new MenuComponent("$ Buy Hardware $", buyFontPos, 1.5f, Color.LawnGreen);
            mcBuy._origin = sf.MeasureString(mcBuy._text) / 2;
            mcBuy.Selected += BuySelected;

            MenuComponent mcPlay = new MenuComponent("Play >>", backFontPos, 1.5f, Color.CornflowerBlue);
            mcPlay._origin = sf.MeasureString(mcPlay._text) / 2;
            mcPlay.Selected += PlaySelected;

            m_mcNoCredits = new MenuComponent("Insuffecient Credits!", insufFontPos, 1.0f, Color.Red);
            m_mcNoCredits._origin = sf.MeasureString(m_mcNoCredits._text) / 2;
            m_mcNoCredits._draw = false;

            m_mcPurchased = new MenuComponent("Hardware Purchased!", insufFontPos, 1.0f, Color.LawnGreen);
            m_mcPurchased._origin = sf.MeasureString(m_mcPurchased._text) / 2;
            m_mcPurchased._draw = false;

            m_mcCredits = new MenuComponent("Credits: " + m_playerShip._credits, creditsFontPos, 1.0f, Color.LawnGreen);
            m_mcCredits._origin = sf.MeasureString(m_mcCredits._text) / 2;

            m_mcShipLife = new MenuComponent("Life: " + m_playerShip._life, shipLifeFontPos, 1.0f, Color.LawnGreen);
            m_mcShipLife._origin = sf.MeasureString(m_mcShipLife._text) / 2;

            m_menuComponenents.Add(mcTitle);
            m_menuComponenents.Add(mcSubText);
            m_menuComponenents.Add(m_mcCredits);
            m_menuComponenents.Add(m_mcShipLife);
            m_menuComponenents.Add(m_mcHealth);
            m_menuComponenents.Add(m_mcIonCannon); 
            m_menuComponenents.Add(m_mcSpreadCannon);
            m_menuComponenents.Add(m_mcGravityWell);
            m_menuComponenents.Add(m_mcShield);
            m_menuComponenents.Add(m_mcStasisRay);
            m_menuComponenents.Add(m_mcNoCredits);
            m_menuComponenents.Add(m_mcPurchased);
            m_menuComponenents.Add(mcBuy);
            m_menuComponenents.Add(mcPlay);
        }

        private void deselectComponent()
        {
            if (m_selectedComponent != null)
            {
                m_selectedComponent._color = Color.White;
                m_selectedComponent = null;
                m_mcNoCredits._draw = false;
            }
        }

        private void selectComponent(HardwareMenuComponent hmc)
        {
            m_mcPurchased._draw = false;
            deselectComponent();
            m_selectedComponent = hmc;
            m_selectedComponent._color = Color.LawnGreen;
            if (m_playerShip._credits < hmc._cost)
            {
                m_mcNoCredits._draw = true;
            }
        }

        private void HealthPackSelected(object sender, EventArgs e)
        {
            selectComponent(m_mcHealth);
        }   

        private void IonCannonSelected(object sender, EventArgs e)
        {
            selectComponent(m_mcIonCannon);
        }

        private void SpreadCannonSelected(object sender, EventArgs e)
        {
            selectComponent(m_mcSpreadCannon);
        }

        private void GravityWellSelected(object sender, EventArgs e)
        {
            selectComponent(m_mcGravityWell);
        }

        private void ShieldSelected(object sender, EventArgs e)
        {
            selectComponent(m_mcShield);
        }

        private void StasisRaySelected(object sender, EventArgs e)
        {
            selectComponent(m_mcStasisRay);
        }

        private void PlaySelected(object sender, EventArgs e)
        {
            m_nextLevelEvent.CreateNextLevelEvent();
        }

        private void BuySelected(object sender, EventArgs e)
        {
            if (m_selectedComponent != null)
            {
                if (m_playerShip._credits >= m_selectedComponent._cost)
                {
                    m_mcPurchased._draw = true;
                    m_buyHardwareEvent.CreateBuyHardwareEvent(m_selectedComponent._type, m_selectedComponent._cost);
                    m_mcCredits._text = "Credits: " + m_playerShip._credits;
                    m_mcShipLife._text = "Life: " + m_playerShip._life;
                    deselectComponent();
                }
            }
        }
    }
}
