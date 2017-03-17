﻿// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;

namespace GVR.Gfx {
  /// <summary>
  /// Useful for drawing a Mesh directly into a target RenderTexture.
  /// </summary>
  public class MeshBlit {
    public static void Render(Mesh mesh, Matrix4x4 meshTRS, Camera camera, RenderTexture targetBuffer,
                              Color clearColor, Material material, int pass = 0) {
      RenderTexture activeRT = RenderTexture.active;
      Graphics.SetRenderTarget(targetBuffer);
      GL.PushMatrix();
      GL.LoadProjectionMatrix(GL.GetGPUProjectionMatrix(camera.projectionMatrix, true));
      if (material.SetPass(pass)) {
        GL.Clear(true, true, clearColor, 1f);
        UnityEngine.Profiling.Profiler.BeginSample("MeshBlit.Render.Draw Mesh Objects");
        for (int i = 0; i < mesh.subMeshCount; i++) {
          Graphics.DrawMeshNow(mesh, meshTRS, i);
        }
        UnityEngine.Profiling.Profiler.EndSample();
      }
      GL.PopMatrix();
      RenderTexture.active = activeRT;
    }


  }
}
