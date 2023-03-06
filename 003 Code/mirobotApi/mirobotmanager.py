from config import *
import asyncio

class MirobotManager():
    def __init__(self, arm) -> None:
        self.arm = arm

    async def AxisControl(self, key) -> None:
        """
        조종 데이터를 기반으로 머니퓰레이터를 제어.
        """
        print(f"{key} pressed, angle : {self.getCurrentAngle()}")

        if key == KUP_1:
           self.arm.set_joint_angle({1:JOINT_ANGLE}, 
                                    is_relative=True, speed= 500, wait_ok=False)        

        elif key == KDOWN_1:
            self.arm.set_joint_angle({1:-JOINT_ANGLE}, is_relative=True)    
                
        elif key == KUP_2:
            self.arm.set_joint_angle({2:JOINT_ANGLE}, is_relative=True)
            
        elif key == KDOWN_2:
            self.arm.set_joint_angle({2:-JOINT_ANGLE}, is_relative=True)            

        elif key == KUP_3:
            self.arm.set_joint_angle({3:JOINT_ANGLE}, is_relative=True) 
                        
        elif key == KDOWN_3:
            self.arm.set_joint_angle({3:-JOINT_ANGLE}, is_relative=True) 

        elif key == KUP_4:
            self.arm.set_joint_angle({4:JOINT_ANGLE}, is_relative=True)
                                                
        elif key == KDOWN_4:
            self.arm.set_joint_angle({4:-JOINT_ANGLE}, is_relative=True)

        elif key == KUP_5:
            self.arm.set_joint_angle({5:JOINT_ANGLE}, is_relative=True) 

        elif key == KDOWN_5:
            self.arm.set_joint_angle({5:-JOINT_ANGLE}, is_relative=True) 

        elif key == KUP_6:
            self.arm.set_joint_angle({6:JOINT_ANGLE}, is_relative=True)

        elif key == KDOWN_6:
            self.arm.set_joint_angle({6:-JOINT_ANGLE}, is_relative=True)
            
        elif key == ENDEF_1:
            self.arm.pump_suction()
            # self.arm.gripper_open()
            
        elif key == ENDEF_2:
            self.arm.pump_blowing()
            # self.arm.gripper_close()
            
        elif key == ENDEF_3:
            self.arm.pump_off()
            
        else:
            print("Unassigned key!") 
    
    def _AxisControl(self, key) -> None:
        """
        조종 데이터를 기반으로 머니퓰레이터를 제어.
        """
        print(f"{key} pressed, angle : {self.getCurrentAngle()}")

        if key == KUP_1:
           self.arm.set_joint_angle({1:JOINT_ANGLE}, 
                                    is_relative=True, speed= 500, wait_ok=False)        

        elif key == KDOWN_1:
            self.arm.set_joint_angle({1:-JOINT_ANGLE}, is_relative=True)    
                
        elif key == KUP_2:
            self.arm.set_joint_angle({2:JOINT_ANGLE}, is_relative=True)
            
        elif key == KDOWN_2:
            self.arm.set_joint_angle({2:-JOINT_ANGLE}, is_relative=True)            

        elif key == KUP_3:
            self.arm.set_joint_angle({3:JOINT_ANGLE}, is_relative=True) 
                        
        elif key == KDOWN_3:
            self.arm.set_joint_angle({3:-JOINT_ANGLE}, is_relative=True) 

        elif key == KUP_4:
            self.arm.set_joint_angle({4:JOINT_ANGLE}, is_relative=True)
                                                
        elif key == KDOWN_4:
            self.arm.set_joint_angle({4:-JOINT_ANGLE}, is_relative=True)

        elif key == KUP_5:
            self.arm.set_joint_angle({5:JOINT_ANGLE}, is_relative=True) 

        elif key == KDOWN_5:
            self.arm.set_joint_angle({5:-JOINT_ANGLE}, is_relative=True) 

        elif key == KUP_6:
            self.arm.set_joint_angle({6:JOINT_ANGLE}, is_relative=True)

        elif key == KDOWN_6:
            self.arm.set_joint_angle({6:-JOINT_ANGLE}, is_relative=True)
            
        elif key == ENDEF_1:
            self.arm.pump_suction()
            # self.arm.gripper_open()
            
        elif key == ENDEF_2:
            self.arm.pump_blowing()
            # self.arm.gripper_close()
            
        elif key == ENDEF_3:
            self.arm.pump_off()
            
        else:
            print("Unassigned key!") 

    def getCurrentPosition(self) -> str:
        """
        미로봇의 현재 좌표를 반환
        """
        self.arm.get_status()
        position = {"x" : self.arm.status.cartesian.x, "y" : self.arm.status.cartesian.y, "z" : self.arm.status.cartesian.z,
                    "roll" : self.arm.status.cartesian.roll, "pitch" : self.arm.status.cartesian.pitch, "yaw" : self.arm.status.cartesian.yaw}
        text = f"Current Position : {position}"
        return text

    def getCurrentAngle(self) -> str:
        """
        미로봇의 현재 관절 각도를 반환
        """
        self.arm.get_status()
        angle = {"1" : self.arm.angle.joint4, "2" : self.arm.angle.joint5, "3" : self.arm.angle.joint6,
                    "4" : self.arm.angle.joint1, "5" : self.arm.angle.joint2, "6" : self.arm.angle.joint3}
        
        return angle
        
    def maxAngle(self, key) -> bool:
        """
        미로봇이 가동할 수 있는 최대각도를 벗어나면 예외처리
        """
        
        self.arm.get_status()
        
        angle = self.getCurrentAngle()
        
        
        if key == KUP_1:
            if angle['1'] > MAX_1:
                return True
            else:
                return False
        
        elif key == KDOWN_1:
            if angle['1'] < MIN_1:
                return True
            else:
                return False
        
        elif key == KUP_2:
            if angle['2'] > MAX_2:
                return True
            else:
                return False
                    
        elif key == KDOWN_2:
            if angle['2'] < MIN_2:
                return True
            else:
                return False
            
        elif key == KUP_3:
            if angle['3'] > MAX_3:
                return True
            else:
                return False
                    
        elif key == KDOWN_3:
            if angle['3'] < MIN_3:
                return True
            else:
                return False            

        elif key == KUP_4:
            if angle['4'] > MAX_4:
                return True
            else:
                return False
                    
        elif key == KDOWN_4:
            if angle['4'] < MIN_4:
                return True
            else:
                return False             
            
        elif key == KUP_5:
            if angle['5'] > MAX_5:
                return True
            else:
                return False
                    
        elif key == KDOWN_5:
            if angle['5'] < MIN_5:
                return True
            else:
                return False 

        elif key == KUP_6:
            if angle['6'] > MAX_6:
                return True
            else:
                return False
                    
        elif key == KDOWN_6:
            if angle['6'] < MIN_6:
                return True
            else:
                return False                               
            
# def jointControl(self, key) -> None:
#         """
#         키보드 이벤트를 입력받아 처리하는 함수
#         """

#         # if self.maxPosition(key):
#         #     print("Can't Move Out of Range!")
            
#         if self.maxAngle(key):
#             print("Can't Move Out of Angle!")            
        
#         else:
#             angle = self.getCurrentAngle()
#             print(f"{key} pressed, angle : {angle}")
            
#             if key == KUP_1:
#                 self.arm.set_joint_angle({1:JOINT_ANGLE}, is_relative=True)         
            
#             elif key == KDOWN_1:
#                 self.arm.set_joint_angle({1:-JOINT_ANGLE}, is_relative=True)    
                    
#             elif key == KUP_2:
#                 self.arm.set_joint_angle({2:JOINT_ANGLE}, is_relative=True)
                
#             elif key == KDOWN_2:
#                 self.arm.set_joint_angle({2:-JOINT_ANGLE}, is_relative=True)            

#             elif key == KUP_3:
#                 self.arm.set_joint_angle({3:JOINT_ANGLE}, is_relative=True) 
                            
#             elif key == KDOWN_3:
#                 self.arm.set_joint_angle({3:-JOINT_ANGLE}, is_relative=True) 

#             elif key == KUP_4:
#                 self.arm.set_joint_angle({4:JOINT_ANGLE}, is_relative=True)
                                                    
#             elif key == KDOWN_4:
#                 self.arm.set_joint_angle({4:-JOINT_ANGLE}, is_relative=True)

#             elif key == KUP_5:
#                 self.arm.set_joint_angle({5:JOINT_ANGLE}, is_relative=True) 

#             elif key == KDOWN_5:
#                 self.arm.set_joint_angle({5:-JOINT_ANGLE}, is_relative=True) 

#             elif key == KUP_6:
#                 self.arm.set_joint_angle({6:JOINT_ANGLE}, is_relative=True)

#             elif key == KDOWN_6:
#                 self.arm.set_joint_angle({6:-JOINT_ANGLE}, is_relative=True)
                
#             elif key == ENDEF_1:
#                 self.arm.pump_suction()
#                 # self.arm.gripper_open()
                
#             elif key == ENDEF_2:
#                 self.arm.pump_blowing()
#                 # self.arm.gripper_close()
             
#             elif key == ENDEF_3:
#                 self.arm.pump_off()
                
#             else:
#                 print("Unassigned key!")    

    # def maxPosition(self, key) -> bool:
    #     """
    #     미로봇이 가동할 수 있는 최대범위를 벗어나면 예외처리
    #     """
    #     self.arm.get_status()
    #     position = {"x" : self.arm.status.cartesian.x, "y" : self.arm.status.cartesian.y, "z" : self.arm.status.cartesian.z,
    #                 "roll" : self.arm.status.cartesian.roll, "pitch" : self.arm.status.cartesian.pitch, "yaw" : self.arm.status.cartesian.yaw}
        
    #     if key == "a" and position == A_MAX or key == "d" and position == D_MAX or \
    #         key == "w" and position == W_MAX or key == "s" and position == S_MAX:
    #         return True
    #     else:
    #         return False  

