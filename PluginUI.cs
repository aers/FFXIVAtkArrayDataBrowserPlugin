﻿using System;
using System.Runtime.InteropServices;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using ImGuiNET;

namespace FFXIVAtkArrayDataBrowserPlugin
{
    public unsafe class PluginUI
    {
        private readonly string pluginName;

        private bool visible = true;

        public bool IsVisible 
        {
            get => this.visible;
            set => this.visible = value;
        }

        public PluginUI(Plugin p)
        {
            pluginName = p.Name;
        }

        public void Draw()
        {
            if (!IsVisible)
                return;

            if (ImGui.Begin($"{pluginName}", ref visible, ImGuiWindowFlags.AlwaysAutoResize))
            {
                var uiModule = Framework.Instance()->GetUiModule();
                if (uiModule == null)
                {
                    ImGui.Text("UIModule unavailable. ");
                    ImGui.End();
                    return;
                }

                ImGui.Text($"UIModule address - {(long)uiModule:X}");
                var atkArrayDataHolder = &uiModule->RaptureAtkModule.AtkModule.AtkArrayDataHolder;
                ImGui.Text($"AtkArrayDataHolder address - {(long)atkArrayDataHolder:X}");
                ImGui.Separator();
                if (ImGui.TreeNode(
                    $"NumberArrayData - array size: {atkArrayDataHolder->NumberArrayCount} - array ptr: {(long)atkArrayDataHolder->NumberArrays:X}###{(long)atkArrayDataHolder->NumberArrays}"))
                {
                    for (int i = 0; i < atkArrayDataHolder->NumberArrayCount; i++)
                    {
                        var numberArrayData = atkArrayDataHolder->NumberArrays[i];
                        if (numberArrayData != null)
                        {
                            if (ImGui.TreeNode(
                                $"index {i} - ptr: {(long)numberArrayData:X}###{(long)numberArrayData}"))
                            {
                                ImGui.Text("Array Numbers: ");
                                for (int j = 0; j < numberArrayData->AtkArrayData.Size; j++)
                                {
                                    ImGui.Text($"index {j} - hex {numberArrayData->IntArray[j]:X} - int {numberArrayData->IntArray[j]} - float {*(float*)&numberArrayData->IntArray[j]}");
                                }

                                ImGui.TreePop();
                            }
                        }
                        else
                        {
                            ImGui.Text($"index {i} - empty");
                        }
                    }

                    ImGui.TreePop();
                }
                if (ImGui.TreeNode(
                    $"StringArrayData - array size: {atkArrayDataHolder->StringArrayCount} - array ptr: {(long)atkArrayDataHolder->StringArrays:X}###{(long)atkArrayDataHolder->StringArrays}"))
                {
                    for (int i = 0; i < atkArrayDataHolder->StringArrayCount; i++)
                    {
                        var stringArrayData = atkArrayDataHolder->StringArrays[i];
                        if (stringArrayData != null)
                        {
                            if (ImGui.TreeNode(
                                $"index {i} - ptr: {(long) stringArrayData:X}###{(long) stringArrayData}"))
                            {
                                ImGui.Text($"Unk String: {Marshal.PtrToStringAuto(new IntPtr(stringArrayData->UnkString))}");
                                ImGui.Text("Array Strings: ");
                                for (int j = 0; j < stringArrayData->AtkArrayData.Size; j++)
                                {
                                    ImGui.Text($"index {j} - {Marshal.PtrToStringAnsi(new IntPtr(stringArrayData->StringArray[j]))}");
                                }

                                ImGui.TreePop();
                            }
                        }
                        else
                        {
                            ImGui.Text($"index {i} - empty");
                        }
                    }

                    ImGui.TreePop();
                }
                ImGui.Text(
                    $"ExtendArrayData - array size: {atkArrayDataHolder->ExtendArrayCount} - array ptr: {(long)atkArrayDataHolder->ExtendArrays:X}");
                ImGui.End();
            }
        }
    }
}
