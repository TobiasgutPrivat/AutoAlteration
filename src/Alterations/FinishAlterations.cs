class OneBack: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,0,32));
        map.PlaceStagedBlocks();
    }
}

class OneForward: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,0,-32));
        map.PlaceStagedBlocks();
    }
}

class OneDown: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,-8,0));
        map.PlaceStagedBlocks();
    }
}

class OneLeft: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(32,0,0));
        map.PlaceStagedBlocks();
    }
}

class OneRight: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(-32,0,0));
        map.PlaceStagedBlocks();
    }
}

class OneUP: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,8,0));
        map.PlaceStagedBlocks();
    }
}

class TwoUP: Alteration {
    public override void Run(Map map){
        map.Move(inventory.Select("Finish|Multilap"), Move(0,16,0));
        map.PlaceStagedBlocks();
    }
}

//better reverse manual

//cp1 is end manual

//TODO floor-fin Macroblock

//TODO Ground-Clippers, Pillars at y=0 get custom finishblock

//TODO inclined tilt start down + remove pillars underneath

//TODO Manslaughter custom finishes

//no gear 5 manual

//TODO Podium customfinish

//puzzle manual

//TODO reverse, customblocks

//TODO Roofing, Macroblock

//short manual

//TODO sky is the finish Macroblock

//TODO there and back

//yep tree puzzle manual
