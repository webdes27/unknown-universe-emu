﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class StarmapStationCommand
    {
        /*
         
public static const ID:int = 26967;
       
      
      public var §_-B4o§:Vector.<§_-Z4L§.§_-w3Y§>;
      
      public var §_-7F§:Number = 0;
      
      public function §_-c30§(param1:Number = 0, param2:Vector.<§_-Z4L§.§_-w3Y§> = null)
      {
         super();
         this.§_-7F§ = param1;
         if(param2 == null)
         {
            this.§_-B4o§ = new Vector.<§_-Z4L§.§_-w3Y§>();
         }
         else
         {
            this.§_-B4o§ = param2;
         }
      }
      
      public function §_-54S§() : int
      {
         return ID;
      }
      
      public function §_-C3W§() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:* = null;
         var _loc2_:int = 0;
         var _loc3_:* = 0;
         while(this.§_-B4o§.length > 0)
         {
            this.§_-B4o§.pop();
         }
         _loc2_ = 0;
         _loc3_ = uint(param1.readInt());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = §_-B4U§.getInstance().createInstance(param1.readShort()) as §_-Z4L§.§_-w3Y§;
            _loc4_.read(param1);
            this.§_-B4o§.push(_loc4_);
            _loc2_++;
         }
         this.§_-7F§ = param1.readDouble();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(ID);
         this.§_-Z4H§(param1);
      }
      
      protected function §_-Z4H§(param1:IDataOutput) : void
      {
         var _loc2_:* = null;
         param1.writeInt(this.§_-B4o§.length);
         for each(_loc2_ in this.§_-B4o§)
         {
            _loc2_.write(param1);
         }
         param1.writeDouble(this.§_-7F§);
      }

         */
    }
}
