namespace KBEngine
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;


    public enum Skill_DisplayType
    {
	    SkillDisplay_Event_Effect,
	    SkillDisplay_Event_Bullet,
	    SkillDisplay_Event_Num,
    }

    public class Skill 
    {
    	public string name;
    	public string descr;
    	public Int32 id;
    	public float canUseDistMin = 0f;
    	public float canUseDistMax = 3f;
        public float coolTime = 0.5f;

        public Skill_DisplayType displayType = Skill_DisplayType.SkillDisplay_Event_Bullet;
        public string skillEffect;
        //技能释放相关
        //private List<SkillStub> skillStubs = new List<SkillStub>();
        //public float castTime = 0.5f;
        
        private float restCoolTimer;
		public Skill()
		{
		}
		//1: 太远， 2：冷却，3：已死亡
		public int validCast(KBEngine.Entity caster, SCObject target)
        {
			float dist = Vector3.Distance(target.getPosition(), caster.position);
            if (dist > canUseDistMax)
                return 1;
            if (restCoolTimer < coolTime)
                return 2;
            if (((SByte)(caster.getDefinedProperty("state"))) == 1)
                return 3;

            return 0;
		}

        public void updateTimer(float second)
        {
            restCoolTimer += second;
        }
		
		public void use(KBEngine.Entity caster, SCObject target)
		{
			caster.cellCall("useTargetSkill", id, ((SCEntityObject)target).targetID);
            restCoolTimer = 0f;
		}
        public void displaySkill(KBEngine.Entity caster, KBEngine.Entity target)
        {
            if (displayType == Skill_DisplayType.SkillDisplay_Event_Bullet)
            {
                UnityEngine.GameObject renderObj = UnityEngine.Object.Instantiate(SkillBox.inst.dictSkillDisplay[skillEffect]) as UnityEngine.GameObject;

                NcEffectFlying fly = renderObj.AddComponent<NcEffectFlying>();
                fly.FromPos = caster.position;
                fly.FromPos.y = 1f;
                fly.ToPos = target.position;
                fly.ToPos.y = 1f;
                //fly.Speed = 5.0f;
                //fly.HWRate = 0;
            }
            else if (displayType == Skill_DisplayType.SkillDisplay_Event_Effect)
            {
                Vector3 pos = target.position;
                pos.y = 1f;
                UnityEngine.Object.Instantiate(SkillBox.inst.dictSkillDisplay[skillEffect], pos, Quaternion.identity);
            }
        }
        public void cast(object renderObj, float distance)
        {
            //SkillStub ss = new SkillStub();
            
            //ss.renderObj = (UnityEngine.GameObject)renderObj;
            //ss.skillDistance = distance;
            //ss.isCasting = true;
            //skillStubs.Add(ss);
        }
    }
} 
